using Azure.Core;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct
{
    internal sealed class CreateProductHandler : ICommandHandler<CreateProduct, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly ILogger<CreateProductHandler> _logger;
        private readonly IContextService _contextService;
        private const string _containerName = "images";

        public CreateProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IManufacturerRepository manufacturerRepository
            , ICategoryRepository categoryRepository, IParameterRepository parameterRepository, ILogger<CreateProductHandler> logger, IContextService contextService)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
            _parameterRepository = parameterRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<Guid> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var manufacturer = await GetManufacturerIfSpecifiedAsync(request.ManufacturerId, cancellationToken);
            var category = await GetCategoryIfSpecifiedAsync(request.CategoryId, cancellationToken);
            var productParameters = await CreateProductParametersAsync(request.ProductParameters, cancellationToken);
            var imageList = await UploadImagesToBlobStorageAsync(request.Images);
            var productId = Guid.NewGuid();
            var product = new Product
                (
                    id: productId,
                    sku: request.SKU,
                    ean: request.EAN,
                    name: request.Name,
                    price: request.Price,
                    vat: request.VAT,
                    quantity: request.Quantity,
                    location: request.Location,
                    description: request.Description,
                    additionalDescription: request.AdditionalDescription,
                    productParameters: productParameters,
                    manufacturer: manufacturer,
                    category: category,
                    images: imageList.ToList(),
                    reserved: request.Quantity is null ? null : 0
                );
            await _productRepository.AddAsync(product);
            _logger.LogInformation("Product was created by {@user}.",
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return productId;
        }
        private async Task<Manufacturer?> GetManufacturerIfSpecifiedAsync(Guid? manufacturerId, CancellationToken cancellationToken)
        {
            if (manufacturerId is null)
                return null;

            var manufacturer = await _manufacturerRepository.GetAsync(manufacturerId.Value, cancellationToken) ?? 
                throw new ManufacturerNotFoundException(manufacturerId.Value);
            return manufacturer;
        }

        private async Task<Category?> GetCategoryIfSpecifiedAsync(Guid? categoryId, CancellationToken cancellationToken)
        {
            if (categoryId is null)
                return null;

            var category = await _categoryRepository.GetAsync(categoryId.Value, cancellationToken) ?? 
                throw new CategoryNotFoundException(categoryId.Value);
            return category;
        }
        private async Task<List<ProductParameter>> CreateProductParametersAsync(
            IEnumerable<ProductParameterDto>? productParameterDtos,
            CancellationToken cancellationToken)
        {
            var productParameters = new List<ProductParameter>();
            if (productParameterDtos is null)
                return productParameters;
            foreach (var dto in productParameterDtos)
            {
                var parameter = await _parameterRepository.GetAsync(dto.ParameterId, cancellationToken)
                    ?? throw new ParameterNotFoundException(dto.ParameterId);
                productParameters.Add(new ProductParameter
                {
                    Parameter = parameter,
                    Value = dto.Value
                });
            }
            return productParameters;
        }
        private async Task<IEnumerable<Image>> UploadImagesToBlobStorageAsync(List<IFormFile> images)
        {
            var imagesList = new List<Image>();
            int counter = 1;
            foreach (var image in images)
            {
                var newGuid = Guid.NewGuid();
                var imageUrlPath = await _blobStorageService.UploadAsync(image, newGuid.ToString(), _containerName);
                imagesList.Add(new Image(newGuid, imageUrlPath, counter++));
            }
            return imagesList;
        }
    }
}
