using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductsHandler : ICommandHandler<DeleteSelectedProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IBlobStorageService _blobStorageService;
        private const string _containerName = "images";

        public DeleteSelectedProductsHandler(IProductRepository productRepository, IImageRepository imageRepository, IBlobStorageService blobStorageService)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _blobStorageService = blobStorageService;
        }
        public async Task Handle(DeleteSelectedProducts request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductsAsync(request.ProductIds);
            await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), _containerName);
            var rowsChanged = await _productRepository.DeleteManyAsync(request.ProductIds);
            if (rowsChanged != request.ProductIds.Length)
            {
                throw new ManufacturerNotAllDeletedException();
            }
        }
    }
}
