using Azure.Core;
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

namespace Ecommerce.Modules.Inventory.Application.Features.Products.CreateProduct
{
    internal sealed class CreateProductHandler : ICommandHandler<CreateProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly TimeProvider _timeProvider;
        private const string _containerName = "files";

        public CreateProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IManufacturerRepository manufacturerRepository
            , ICategoryRepository categoryRepository, TimeProvider timeProvider)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _manufacturerRepository = manufacturerRepository;
            _categoryRepository = categoryRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var imageList = await UploadImagesToBlobStorage(request.Images);
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
            foreach (var productParameter in request.ProductParameters)
            {
                productParameters.Add(new ProductParameter()
                {
                    ParameterId = productParameter.ParameterId,
                    Value = productParameter.Value
                });
            }
            var rowChanged = await _productRepository.AddAsync(new Product()
            {
                SKU = request.SKU,
                EAN = request.EAN,
                Name = request.Name,
                Price = request.Price,
                VAT = request.VAT,
                Quantity = request.Quantity,
                Location = request.Location,
                Description = request.Description,
                AdditionalDescription = request.AdditionalDescription,
                ProductParameters = productParameters,
                Manufacturer = manufacturer,
                Category = category,
                Images = imageList.ToList(),
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
            });
        }
        private async Task<IEnumerable<Image>> UploadImagesToBlobStorage(IFormFile[] images)
        {
            var imagesList = new List<Image>();
            int counter = 1;
            foreach (var image in images)
            {
                var imageUrlPath = await _blobStorageService.UploadAsync(image, _containerName);
                imagesList.Add(new Image()
                {
                    Id = Guid.NewGuid(),
                    ImageUrlPath = imageUrlPath,
                    Order = counter++
                });
            }
            return imagesList;
        }
    }
}
