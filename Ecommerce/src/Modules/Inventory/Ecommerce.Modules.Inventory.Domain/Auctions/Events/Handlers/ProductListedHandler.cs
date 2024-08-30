﻿using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Events.Handlers
{
    public sealed class ProductListedHandler : IDomainEventHandler<ProductListed>
    {
        private readonly IAuctionRepository _auctionRepository;

        public ProductListedHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task HandleAsync(ProductListed @event)
        {
            var products = @event.Products;
            var auctions = new List<Auction>();
            foreach (var product in products)
            {
                auctions.Add(new Auction(
                    id: product.Id,
                    sku: product.SKU,
                    name: product.Name,
                    price: product.Price,
                    description: product.Description,
                    imagePathUrls: product.Images.Select(i => i.ImageUrlPath).ToList(),
                    category: product.Category.Name,
                    createdAt: @event.ListedAt,
                    quantity: product.Quantity,
                    additionalDescription: product.AdditionalDescription,
                    parameters: product.ProductParameters?.Select(pp => new Entities.AuctionParameter(pp.Parameter.Name, pp.Value)).ToList(),
                    manufacturer: product.Manufacturer.Name
                ));
            }
            var rowsChanged = await _auctionRepository.AddManyAsync(auctions);
            if(rowsChanged != products.Count())
            {
                throw new AuctionNotListedException();
            }
        }
    }
}
