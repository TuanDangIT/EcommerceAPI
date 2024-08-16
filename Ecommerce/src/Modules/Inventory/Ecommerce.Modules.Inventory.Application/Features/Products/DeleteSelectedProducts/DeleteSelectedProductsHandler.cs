using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Domain.Entities;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductsHandler : ICommandHandler<DeleteSelectedProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IBlobStorageService _blobStorageService;
        private const string containerName = "images";

        public DeleteSelectedProductsHandler(IProductRepository productRepository, IImageRepository imageRepository, IBlobStorageService blobStorageService)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _blobStorageService = blobStorageService;
        }
        public async Task Handle(DeleteSelectedProducts request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductsAsync(request.ProductIds);
            await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), containerName);
            var rowsChanged = await _productRepository.DeleteManyAsync(request.ProductIds);
            if (rowsChanged != request.ProductIds.Count())
            {
                throw new ManufacturerNotAllDeletedException();
            }
        }
    }
}
