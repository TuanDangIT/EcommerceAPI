using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External.Handlers
{
    internal class CustomerPlacedOrderHandler : IEventHandler<CustomerPlacedOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly TimeProvider _timeProvider;

        public CustomerPlacedOrderHandler(IOrderRepository orderRepository, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _timeProvider = timeProvider;
        }
        public async Task HandleAsync(CustomerPlacedOrder @event)
        {
            var products = new List<Product>();
            foreach (var productObject in @event.Products)
            {
                var product = JsonSerializer.Deserialize<Product>(JsonSerializer.Serialize(productObject));
                if (product is null)
                {
                    throw new ArgumentNullException(nameof(@event));
                }
                products.Add(product);
            }
            var newGuid = Guid.NewGuid();
            var order = new Order(
                newGuid, 
                products, 
                new Shipment(@event.City, @event.PostalCode, @event.StreetName, @event.StreetNumber, @event.AparmentNumber, @event.ReceiverFullName),
                (PaymentMethod)Enum.Parse(typeof(PaymentMethod), @event.PaymentMethod),
                _timeProvider.GetUtcNow().UtcDateTime,
                @event.CustomerId
                );
            await _orderRepository.CreateOrder(order);
        }
    }
}
