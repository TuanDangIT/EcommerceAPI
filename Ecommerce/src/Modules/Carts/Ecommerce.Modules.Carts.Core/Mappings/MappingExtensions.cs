using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Mappings
{
    internal static class MappingExtensions
    {
        public static CartDto AsDto(this Cart cart)
            => new CartDto()
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                Products = cart.Products.Select(p => p.AsDto())
            };
        public static CheckoutCartDto AsDto(this CheckoutCart cart)
            => new CheckoutCartDto()
            {
                Id = cart.Id,
                Customer = cart.Customer.AsDto(),
                Payment = cart.Payment?.AsDto(),
                Shipment = cart.Shipment?.AsDto(),
                IsPaid = cart.IsPaid,
                Products = cart.Products.Select(p => p.AsDto()),
            };
        public static CartProductDto AsDto(this CartProduct product)
            => new CartProductDto()
            {
                Id = product.Id,
                ProductId = product.ProductId,
                Name = product.Product.Name,
                Price = product.Product.Price,
                ImagePathUrl = product.Product.ImagePathUrl,
                Quantity = product.Quantity
            };
        public static PaymentDto AsDto(this Payment payment)
            => new PaymentDto()
            {
                Id = payment.Id,
                PaymentMethod = payment.PaymentMethod.ToString(),
            };
        public static CheckoutStripeSessionDto AsDto(this Session session)
            => new CheckoutStripeSessionDto()
            {
                SessionId = session.Id,
                CheckoutUrl = session.Url
            };
        public static ShipmentDto AsDto(this Shipment shipment)
            => new ShipmentDto()
            {
                City = shipment.City,
                PostalCode = shipment.PostalCode,
                StreetName = shipment.StreetName,
                StreetNumber = shipment.StreetNumber,
                AparmentNumber = shipment.AparmentNumber
            };
        public static CustomerDto AsDto(this Customer customer)
            => new CustomerDto()
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
    }
}
