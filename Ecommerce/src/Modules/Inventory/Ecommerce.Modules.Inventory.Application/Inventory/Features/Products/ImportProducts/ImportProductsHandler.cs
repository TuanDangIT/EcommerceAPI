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
            if (request.ImportFile.ContentType != MediaTypeNames.Text.Csv)
            {
                throw new ImportFileNotSupportedException(request.ImportFile.ContentType);
            }
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
                Delimiter = request.Delimiter.ToString()
            };
            using var reader = new StreamReader(request.ImportFile.OpenReadStream());
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap(new ProductCsvClassMap(request.Delimiter is ',' ? ';' : ','));
            var productRecords = csv.GetRecords<ProductCsvRecordDto>().ToList();
            var existingParameters = (await _parameterRepository.GetAllAsync(cancellationToken)).ToDictionary(p => p.Name);
            var existingCategories = (await _categoryRepository.GetAllAsync(cancellationToken)).ToDictionary(c => c.Name);
            var existingManufacturers = (await _manufacturerRepository.GetAllAsync(cancellationToken)).ToDictionary(m => m.Name);
            var newManufacturers = new Dictionary<string, Manufacturer>();
            var newCategories = new Dictionary<string, Category>();
            var newParameters = new Dictionary<string, Parameter>();
            var products = new List<Product>();
            foreach (var record in productRecords)
            {
                if (!existingManufacturers.TryGetValue(record.Manufacturer, out var manufacturer))
                {
                    if (!newManufacturers.TryGetValue(record.Manufacturer, out manufacturer))
                    {
                        manufacturer = new Manufacturer(record.Manufacturer);
                        newManufacturers[record.Manufacturer] = manufacturer;
                    }
                }
                if (!existingCategories.TryGetValue(record.Category, out var category))
                {
                    if (!newCategories.TryGetValue(record.Category, out category))
                    {
                        category = new Category(record.Category);
                        newCategories[record.Category] = category;
                    }
                }
                var productParameters = new List<ProductParameter>();
                foreach (var (paramName, value) in record.Parameters)
                {
                    if (!existingParameters.TryGetValue(paramName, out var parameter))
                    {
                        if (!newParameters.TryGetValue(paramName, out parameter))
                        {
                            parameter = new Parameter(paramName);
                            newParameters[paramName] = parameter;
                        }
                    }
                    productParameters.Add(new ProductParameter
                    {
                        Parameter = parameter,
                        Value = value
                    });
                }

                var product = new Product(
                    record.SKU,
                    record.Name,
                    record.Price,
                    record.VAT,
                    record.Description,
                    productParameters,
                    manufacturer,
                    category,
                    record.Images.Select((url, index) => new Image(Guid.NewGuid(), url, index)).ToList(),
                    record.EAN,
                    record.Quantity,
                    record.Location,
                    record.AdditionalDescription,
                    record.Quantity is null ? null : 0
                );
                products.Add(product);
            }
            using var transaction = _inventoryUnitOfWork.BeginTransaction();
            try
            {
                if (newManufacturers.Count != 0)
                    await _manufacturerRepository.AddManyAsync(newManufacturers.Values, cancellationToken);

                if (newCategories.Count != 0)
                    await _categoryRepository.AddManyAsync(newCategories.Values, cancellationToken);

                if (newParameters.Count != 0)
                    await _parameterRepository.AddManyAsync(newParameters.Values, cancellationToken);
                await _productRepository.AddManyAsync(products, cancellationToken);
                transaction.Commit();
                _logger.LogInformation("Producs were imported by {@user}", 
                    new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            }
            catch(Exception e)
            {
                transaction.Rollback();
                _logger.LogError("Import products operation that was called by {@user} failed. Exception: {@exception}", 
                    new { _contextService.Identity!.Username, _contextService.Identity!.Id }, e);
                throw;
            }
        }
    }
}
