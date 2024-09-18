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
            //try serialize whole list
            foreach (var productObject in @event.Products)
            {
                var serializedObject = JsonSerializer.Serialize(productObject);
                var product = JsonSerializer.Deserialize<Product>(serializedObject);
                if (product is null)
                {
                    throw new ArgumentNullException(nameof(@event));
                }
                products.Add(product);
            }
            var newGuid = Guid.NewGuid();
            var customer = new Customer(@event.FirstName, @event.LastName, @event.Email, @event.PhoneNumber, @event.CustomerId);
            var shipment = new Shipment(@event.City, @event.PostalCode, @event.StreetName, @event.StreetNumber, @event.ApartmentNumber);
            var paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), @event.PaymentMethod);
            var additionalInformation = @event.AdditionalInformation;
            var order = new Order(
                newGuid,
                customer, 
                products, 
                shipment,
                paymentMethod,
                _timeProvider.GetUtcNow().UtcDateTime,
                additionalInformation
                );
            await _orderRepository.CreateOrderAsync(order);
        }
    }
}
