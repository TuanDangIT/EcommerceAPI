using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Entities;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.UpdateProduct
{
    internal sealed class UpdateProductHandler : ICommandHandler<UpdateProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly TimeProvider _timeProvider;
        private readonly IParameterRepository _parameterRepository;
        private const string _containerName = "images";

        public UpdateProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IManufacturerRepository manufacturerRepository
            , ICategoryRepository categoryRepository, TimeProvider timeProvider, IParameterRepository parameterRepository)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
            _timeProvider = timeProvider;
            _parameterRepository = parameterRepository;
        }
        public async Task Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.Id);
            if(product is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            var imageList = await UploadImagesToBlobStorageAsync(request.Images);
            var manufacturer = await _manufacturerRepository.GetAsync(request.ManufacturerId);
            if (manufacturer is null)
            {
                throw new ManufacturerNotFoundException(request.ManufacturerId);
            }
            var category = await _categoryRepository.GetAsync(request.CategoryId);
            if (category is null)
            {
                throw new CategoryNotFoundException(request.CategoryId);
            }
            var productParameters = new List<ProductParameter>();
            if (request.ProductParameters is not null)
            {
                foreach (var productParameter in request.ProductParameters)
                {
                    var parameter = await _parameterRepository.GetAsync(productParameter.ParameterId);
                    if (parameter is null)
                    {
                        throw new ParameterNotFoundException(productParameter.ParameterId);
                    }
                    productParameters.Add(new ProductParameter()
                    {
                        Parameter = parameter,
                        Value = productParameter.Value,
                        CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
                    });
                }
            }
            product.Edit
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
                    productParameters: productParameters,
                    manufacturer: manufacturer,
                    category: category,
                    images: imageList.ToList(),
                    updatedAt: _timeProvider.GetUtcNow().UtcDateTime
                );
            var rowsChanged = await _productRepository.UpdateAsync(product);
            if (rowsChanged is 0)
            {
                throw new ProductNotUpdatedException(request.Id);
            }
        }
        private async Task<IEnumerable<Image>> UploadImagesToBlobStorageAsync(List<IFormFile> images)
        {
            var imagesList = new List<Image>();
            int counter = 1;
            foreach (var image in images)
            {
                var newGuid = Guid.NewGuid();
                var imageUrlPath = await _blobStorageService.UploadAsync(image, newGuid.ToString(), _containerName);
                imagesList.Add(new Image()
                {
                    Id = newGuid,
                    ImageUrlPath = imageUrlPath,
                    Order = counter++
                });
            }
            return imagesList;
        }
    }
}
