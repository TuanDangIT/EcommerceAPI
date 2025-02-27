using CsvHelper;
using CsvHelper.Configuration;
using Ecommerce.Modules.Inventory.Application.DAL;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts
{
    internal class ImportProductsHandler : ICommandHandler<ImportProducts>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IProductRepository _productRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly ILogger<ImportProductsHandler> _logger;
        private readonly IContextService _contextService;

        public ImportProductsHandler(ICategoryRepository categoryRepository, IParameterRepository parameterRepository,
            IProductRepository productRepository, IManufacturerRepository manufacturerRepository, IInventoryUnitOfWork inventoryUnitOfWork,
            ILogger<ImportProductsHandler> logger, IContextService contextService)
        {
            _categoryRepository = categoryRepository;
            _parameterRepository = parameterRepository;
            _productRepository = productRepository;
            _manufacturerRepository = manufacturerRepository;
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ImportProducts request, CancellationToken cancellationToken)
        {
            ValidateImportFile(request);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
                Delimiter = request.Delimiter.ToString()
            };
            var productRecords = ParseCsvFile(request, cancellationToken);
            if (productRecords.Count == 0)
            {
                _logger.LogInformation("No products found in the import file by user {@user}.", new { _contextService.Identity!.Username, _contextService.Identity!.Id });
                return;
            }
            await ProcessImportData(productRecords, cancellationToken);
        }
        private void ValidateImportFile(ImportProducts request)
        {
            if (request.ImportFile == null)
            {
                throw new ArgumentNullException(nameof(request.ImportFile), "Import file is required");
            }

            if (request.ImportFile.ContentType != MediaTypeNames.Text.Csv)
            {
                throw new ImportFileNotSupportedException(request.ImportFile.ContentType);
            }
        }
        private List<ProductCsvRecordDto> ParseCsvFile(ImportProducts request, CancellationToken cancellationToken)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
                Delimiter = request.Delimiter.ToString(),
                BadDataFound = null 
            };

            using var reader = new StreamReader(request.ImportFile.OpenReadStream());
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap(new ProductCsvClassMap(request.Delimiter is ',' ? ';' : ','));
            return csv.GetRecords<ProductCsvRecordDto>().ToList();
        }
        private T GetOrCreate<T>(string key, Dictionary<string, T> existing, Dictionary<string, T> newItems, Func<string, T> factory)
        {
            if (existing.TryGetValue(key, out var item) || newItems.TryGetValue(key, out item))
            {
                return item;
            }

            item = factory(key);
            newItems[key] = item;
            return item;
        }
        private async Task ProcessImportData(List<ProductCsvRecordDto> productRecords, CancellationToken cancellationToken)
        {
            var existingParameters = (await _parameterRepository.GetAllAsync(cancellationToken)).ToDictionary(p => p.Name);
            var existingCategories = (await _categoryRepository.GetAllAsync(cancellationToken)).ToDictionary(c => c.Name);
            var existingManufacturers = (await _manufacturerRepository.GetAllAsync(cancellationToken)).ToDictionary(m => m.Name);
            var newManufacturers = new Dictionary<string, Manufacturer>();
            var newCategories = new Dictionary<string, Category>();
            var newParameters = new Dictionary<string, Parameter>();
            var products = new List<Product>();

            foreach (var record in productRecords)
            {
                var manufacturer = GetOrCreate(
                            record.Manufacturer,
                            existingManufacturers,
                            newManufacturers,
                            name => new Manufacturer(name)
                        );

                var category = GetOrCreate(
                    record.Category,
                    existingCategories,
                    newCategories,
                    name => new Category(name)
                );

                var productParameters = new List<ProductParameter>();
                foreach (var (paramName, value) in record.Parameters)
                {
                    if (string.IsNullOrWhiteSpace(paramName))
                        continue;

                    var parameter = GetOrCreate(
                        paramName,
                        existingParameters,
                        newParameters,
                        name => new Parameter(name)
                    );

                    productParameters.Add(new ProductParameter
                    {
                        Parameter = parameter,
                        Value = value
                    });
                }

                var images = record.Images
                    .Select((url, index) => new Image(Guid.NewGuid(), url, index))
                    .ToList();

                var product = new Product(
                    record.SKU,
                    record.Name,
                    record.Price,
                    record.VAT,
                    record.Description,
                    productParameters,
                    manufacturer,
                    category,
                    images,
                    record.EAN,
                    record.Quantity,
                    record.Location,
                    record.AdditionalDescription,
                    record.Quantity is null ? null : 0
                );

                products.Add(product);
            }

            if (products.Count == 0)
            {
                _logger.LogInformation("No products found in the import file by user {@user}.", new { _contextService.Identity!.Username, _contextService.Identity!.Id });
                return;
            }

            using var transaction = _inventoryUnitOfWork.BeginTransaction();
            try
            {
                var saveTasks = new List<Task>();

                if (newManufacturers.Count > 0)
                    await _manufacturerRepository.AddManyAsync(newManufacturers.Values, cancellationToken);

                if (newCategories.Count > 0)
                    await _categoryRepository.AddManyAsync(newCategories.Values, cancellationToken);

                if (newParameters.Count > 0)
                    await _parameterRepository.AddManyAsync(newParameters.Values, cancellationToken);

                await _productRepository.AddManyAsync(products, cancellationToken);

                transaction.Commit();

                _logger.LogInformation("Products were imported by {@user}.",
                    new { _contextService.Identity?.Username, _contextService.Identity?.Id });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogInformation(e, "Import products operation that was called by {@user} failed.",
                    new { _contextService.Identity?.Username, _contextService.Identity?.Id });
                throw;
            }
        }
    }
}
