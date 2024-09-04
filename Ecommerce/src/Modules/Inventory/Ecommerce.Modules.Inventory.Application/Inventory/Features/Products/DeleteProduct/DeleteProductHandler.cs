﻿using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private const string _containerName = "images";

        public DeleteProductHandler(IProductRepository productRepository, IBlobStorageService blobStorageService, IImageRepository imageRepository)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _imageRepository = imageRepository;
        }
        public async Task Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var imagesIds = await _imageRepository.GetAllImagesForProductAsync(request.ProductId);
            await _blobStorageService.DeleteManyAsync(imagesIds.Select(i => i.ToString()), _containerName);
            await _productRepository.DeleteAsync(request.ProductId);
        }
    }
}
