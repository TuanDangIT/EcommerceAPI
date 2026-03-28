using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Events.Externals;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External.Handlers
{
    internal class ProductsListedHandler : IEventHandler<ProductsListed>
    {
        private readonly IProductRepository _productRepository;

        public ProductsListedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task HandleAsync(ProductsListed @event)
        {
            var products = @event.Products.Select(p => new Product(p.SKU, p.Name, p.Price, p.Quantity, p.ImagePathUrl)).ToList();
             await _productRepository.AddAsync(products);       
        }
    }
}
