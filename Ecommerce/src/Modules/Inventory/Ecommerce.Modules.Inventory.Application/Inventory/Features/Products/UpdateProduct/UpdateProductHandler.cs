using Azure.Core;
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
            var manufacturer = await _manufacturerRepository.GetAsync(request.ManufacturerId, cancellationToken) ?? 
                throw new ManufacturerNotFoundException(request.ManufacturerId);
            var category = await _categoryRepository.GetAsync(request.CategoryId, cancellationToken) ?? 
                throw new CategoryNotFoundException(request.CategoryId);
            var productParameters = new List<ProductParameter>();
            if (request.ProductParameters is not null)
            {
                foreach (var productParameter in request.ProductParameters)
                {
                    var parameter = await _parameterRepository.GetAsync(productParameter.ParameterId, cancellationToken) ?? 
                        throw new ParameterNotFoundException(productParameter.ParameterId);
                    productParameters.Add(new ProductParameter()
                    {
                        Parameter = parameter,
                        Value = productParameter.Value,
                    });
                }
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
            product.ChangeManufacturer(manufacturer);
            product.ChangeCategory(category);
            await _productRepository.DeleteProductParametersAndImagesRelatedToProduct(request.Id, cancellationToken);
            product.ChangeParameters(productParameters);
            await _productRepository.UpdateAsync();
            await UploadImagesToBlobStorageAsync(request.Images, product);
            _logger.LogInformation("Product: {productId} was updated with new details {@request} by {@user}.",
                product.Id, request, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
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
