using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DeleteProduct
{
    internal sealed class DeleteProductHandler : ICommandHandler<DeleteProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IImageRepository _imageRepository;
        private const string containerName = "images";

        public DeleteProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IImageRepository imageRepository)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _imageRepository = imageRepository;
        }
        public async Task Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductAsync(request.ProductId);
            await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), containerName);
            var rowsChanged = await _productRepository.DeleteAsync(request.ProductId);
            if(rowsChanged is 0)
            {
                throw new ProductNotDeletedException(request.ProductId);
            }
        }
    }
}
