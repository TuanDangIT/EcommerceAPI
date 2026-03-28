using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External.Handlers
{
    internal class ProductsUnlistedHandler : IEventHandler<ProductsUnlisted>
    {
        private readonly IProductRepository _productRepository;

        public ProductsUnlistedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task HandleAsync(ProductsUnlisted @event)
        {
            await _productRepository.DeleteAsync([.. @event.ProductIds]);
        }
    }
}
