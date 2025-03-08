using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Services;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ListProducts
{
    internal sealed class ListProductsHandler : ICommandHandler<ListProducts>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;
        private readonly IInventoryEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<ListProductsHandler> _logger;
        private readonly IContextService _contextService;

        public ListProductsHandler(IProductRepository productRepository, IDomainEventDispatcher domainEventDispatcher,
            TimeProvider timeProvider, IInventoryEventMapper eventMapper, IMessageBroker messageBroker, ILogger<ListProductsHandler> logger,
            IContextService contextService)
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ListProducts request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(request.ProductIds, cancellationToken,
                query => query.Include(p => p.Manufacturer),
                query => query.Include(p => p.Category),
                query => query.Include(p => p.Images.OrderBy(i => i.Order)),
                query => query.Include(p => p.ProductParameters).ThenInclude(pp => pp.Parameter));
            //if (products.Count() != request.ProductIds.Length)
            //{
            //    var productIdsNotFound = new List<Guid>();
            //    foreach(var productId in request.ProductIds)
            //    {
            //        if(products.Select(p => p.Id).Contains(productId))
            //        {
            //            productIdsNotFound.Add(productId);
            //        }
            //    }
            //    throw new ProductNotAllFoundException(productIdsNotFound);
            //}
            var foundProductIds = products.Select(p => p.Id).ToHashSet();
            var missingProductIds = request.ProductIds.Where(id => !foundProductIds.Contains(id)).ToList();

            if (missingProductIds.Any())
            {
                throw new ProductNotAllFoundException(missingProductIds);
            }
            var productsToList = new List<Product>();
            foreach(var product in products)
            {
                if(product.IsListed == false)
                {
                    productsToList.Add(product);
                }
                else
                {
                    _logger.LogWarning("Product: {productId} was not listed, because it's been already listed.", product.Id);
                }
            }
            if (!productsToList.Any())
            {
                _logger.LogWarning("No products needed to be listed.");
                return;
            }
            var productIdsToList = productsToList.Select(p => p.Id).ToArray();
            await _productRepository.UpdateListedFlagAsync(productIdsToList, true, cancellationToken);
            _logger.LogInformation("Products: {@productIds} were listed by {@user}.",
                productIdsToList, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            var domainEvent = new ProductsListed(productsToList, _timeProvider.GetUtcNow().UtcDateTime);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _eventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
