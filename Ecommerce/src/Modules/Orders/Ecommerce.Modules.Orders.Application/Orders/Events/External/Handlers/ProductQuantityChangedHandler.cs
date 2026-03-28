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
    internal class ProductQuantityChangedHandler : IEventHandler<ProductQuantityChanged>
    {
        private readonly IProductRepository _productRepository;

        public ProductQuantityChangedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task HandleAsync(ProductQuantityChanged @event)
        {
            throw new NotImplementedException();
        }
    }
}
