using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProduct
{
    internal sealed class ListProductHandler : ICommandHandler<ListProduct>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;

        public ListProductHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            TimeProvider timeProvider)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ListProduct request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(request.ProductIds);
            if(products.Count() != request.ProductIds.Length)
            {
                var productIdsNotFound = new List<Guid>();
                foreach(var productId in request.ProductIds)
                {
                    if(products.Select(p => p.Id).Contains(productId))
                    {
                        productIdsNotFound.Add(productId);
                    }
                }
                throw new ProductNotAllFoundException(productIdsNotFound);
            }
            await _domainEventDispatcher.DispatchAsync(new ProductListed(products, _timeProvider.GetUtcNow().UtcDateTime));
        }
    }
}
