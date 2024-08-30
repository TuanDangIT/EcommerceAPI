using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UnlistProduct
{
    public sealed class UnlistProductHandler : ICommandHandler<UnlistProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public UnlistProductHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task Handle(UnlistProduct request, CancellationToken cancellationToken)
        {
            var productIds = await _productRepository.GetAllIdThatContainsInArrayAsync(request.ProductIds);
            if (productIds.Count() != request.ProductIds.Length)
            {
                var productIdsNotFound = new List<Guid>();
                foreach (var productId in request.ProductIds)
                {
                    if (productIds.Contains(productId))
                    {
                        productIdsNotFound.Add(productId);
                    }
                }
                throw new ProductNotAllFoundException(productIdsNotFound);
            }
            await _domainEventDispatcher.DispatchAsync(new ProductUnlisted(productIds));
        }
    }
}
