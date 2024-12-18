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
                        Value = productParameter.Value
                    });
                }
            }
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
            _logger.LogInformation("Product: {@product} was created by {@user}.",
                product, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return productId;
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
