using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteProduct
{
    internal sealed class DeleteProductHandler : ICommandHandler<DeleteProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<DeleteProductHandler> _logger;
        private readonly IContextService _contextService;
        private const string _containerName = "images";

        public DeleteProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IImageRepository imageRepository,
            ILogger<DeleteProductHandler> logger, IContextService contextService)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _imageRepository = imageRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductAsync(request.ProductId, cancellationToken);
            if(imagesIds.Any())
            {
                await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), _containerName, cancellationToken);
            }
            await _productRepository.DeleteAsync(request.ProductId);
            _logger.LogInformation("Product: {productId} was deleted by {@user}.",
                request.ProductId, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
