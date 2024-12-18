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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductsHandler : ICommandHandler<DeleteSelectedProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DeleteSelectedProductsHandler> _logger;
        private readonly IContextService _contextService;
        private const string _containerName = "images";

        public DeleteSelectedProductsHandler(IProductRepository productRepository, IImageRepository imageRepository, IBlobStorageService blobStorageService,
            ILogger<DeleteSelectedProductsHandler> logger, IContextService contextService)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _blobStorageService = blobStorageService;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteSelectedProducts request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductsAsync(request.ProductIds, cancellationToken);
            await _productRepository.DeleteManyAsync(cancellationToken, request.ProductIds);
            await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), _containerName);
            _logger.LogInformation("Products: {productIds} were deleted by {@user}.",
                request.ProductIds, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
