using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External.Handlers
{
    internal class CartClearedHandler : IEventHandler<CartCleared>
    {
        private readonly IProductRepository _productRepository;

        public CartClearedHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task HandleAsync(CartCleared @event)
        {
            var products = await _productRepository.GetAllThatContainsInArrayAsync(@event.Products.Select(p => p.Id).ToArray());
            foreach (var product in products)
            {
                if (product.HasQuantity)
                {
                    var soldQuantity = @event.Products
                        .Single(p => p.Id == product.Id)
                        .Quantity;
                    product.DecreaseQuantity((int)soldQuantity!);
                }
            }
            await _productRepository.UpdateAsync();
        }
    }
}
