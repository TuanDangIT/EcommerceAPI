using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public CustomerPlacedOrderHandler(IOrderRepository orderRepository, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task HandleAsync(CustomerPlacedOrder @event)
        {
            //var products = new List<Product>();
            //try serialize whole list
            //foreach (var productObject in @event.Products)
            //{
            //    var serializedObject = JsonSerializer.Serialize(productObject);
            //    var product = JsonSerializer.Deserialize<Product>(serializedObject);
            //    if (product is null)
            //    {
            //        throw new ArgumentNullException(nameof(@event));
            //    }
            //    products.Add(product);
            //}
            var newGuid = Guid.NewGuid();
            var customer = new Customer(@event.FirstName, @event.LastName, @event.Email, @event.PhoneNumber, @event.CustomerId);
            //var shipment = new ShipmentDetails(@event.City, @event.PostalCode, @event.StreetName, @event.StreetNumber, @event.ApartmentNumber);
            var address = new Address(@event.StreetName, @event.StreetNumber + "/" + @event.ApartmentNumber, @event.City, @event.PostalCode);
            var receiver = new Receiver(@event.FirstName, @event.LastName, @event.Email, @event.PhoneNumber, address);
            var shipment = new Shipment(receiver, @event.TotalSum);
            if(!Enum.TryParse(typeof(PaymentMethod), @event.PaymentMethod, out var paymentMethod))
            {
                throw new InvalidPaymentMethodException();
            }
            var additionalInformation = @event.AdditionalInformation;
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            var order = new Order(
                newGuid,
                customer, 
                @event.Products, 
                @event.TotalSum,
                shipment,
                (PaymentMethod)paymentMethod!,
                now,
                additionalInformation,
                @event.DiscountCode,
                @event.StripePaymentIntentId
                );
            await _orderRepository.CreateAsync(order);
            await _messageBroker.PublishAsync(new OrderCreated(newGuid, customer.UserId, customer.FirstName, customer.Email, @event.Products.Select(p => new { p.Name, p.SKU, p.Price, p.Quantity }), order.TotalSum, now));
        }
    }
}
