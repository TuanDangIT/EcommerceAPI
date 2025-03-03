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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UpdateProduct
{
    internal sealed class UpdateProductHandler : ICommandHandler<UpdateProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IParameterRepository _parameterRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<UpdateProductHandler> _logger;
        private readonly IContextService _contextService;
        private const string _containerName = "images";

        public UpdateProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IManufacturerRepository manufacturerRepository,
            ICategoryRepository categoryRepository, IParameterRepository parameterRepository, IImageRepository imageRepository, ILogger<UpdateProductHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
            _parameterRepository = parameterRepository;
            _imageRepository = imageRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.Id, cancellationToken) ?? 
                throw new ProductNotFoundException(request.Id);
            if (product.IsListed)
            {
                throw new CannotUpdateListedProductException(request.Id);
            }
            product.ChangeBaseDetails
                (
                    sku: request.SKU,
                    ean: request.EAN,
                    name: request.Name,
                    price: request.Price,
                    vat: request.VAT,
                    quantity: request.Quantity,
                    location: request.Location,
                    description: request.Description,
                    additionalDescription: request.AdditionalDescription,
                    reserved: request.Reserved
                );
            var manufacturer = await GetManufacturerIfSpecifiedAsync(request.ManufacturerId, cancellationToken);
            var category = await GetCategoryIfSpecifiedAsync(request.CategoryId, cancellationToken);
            product.ChangeManufacturer(manufacturer);
            product.ChangeCategory(category);
            var productParameters = await ChangeProductParametersAsync(request.ProductParameters, cancellationToken);
            product.ChangeParameters(productParameters);
            await _productRepository.DeleteProductParametersAndImagesRelatedToProduct(request.Id, cancellationToken);
            await _productRepository.UpdateAsync();
            await UploadImagesToBlobStorageAsync(request.Images, product);
            _logger.LogInformation("Product: {productId} was updated with new details {@request} by {@user}.",
                product.Id, request, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
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
        private async Task<List<ProductParameter>> ChangeProductParametersAsync(
            IEnumerable<ProductParameterDto>? productParameterDtos,
            CancellationToken cancellationToken)
        {
            var productParameters = new List<ProductParameter>();
            if (productParameterDtos is null)
                return productParameters;
            foreach (var dto in productParameterDtos)
            {
                var parameter = await _parameterRepository.GetAsync(dto.ParameterId, cancellationToken) ??
                    throw new ParameterNotFoundException(dto.ParameterId);
                productParameters.Add(new ProductParameter
                {
                    Parameter = parameter,
                    Value = dto.Value
                });
            }
            return productParameters;
        }
        private async Task UploadImagesToBlobStorageAsync(List<IFormFile> images, Product product)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductAsync(product.Id);
            if (imagesIds.Any())
            {
                await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), _containerName);
            }
            var imagesList = new List<Image>();
            int counter = 1;
            foreach (var image in images)
            {
                var newGuid = Guid.NewGuid();
                var imageUrlPath = await _blobStorageService.UploadAsync(image, newGuid.ToString(), _containerName);
                imagesList.Add(new Image(newGuid, imageUrlPath, counter++, product));
            }
            await _imageRepository.AddRangeAsync(imagesList);
        }
    }
}
