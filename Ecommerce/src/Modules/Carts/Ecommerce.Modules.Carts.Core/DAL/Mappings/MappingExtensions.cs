using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.ValueObjects;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static CartDto AsDto(this Cart cart)
            => new()
            {
                Id = cart.Id,
                Products = cart.Products.Select(p => p.AsDto()),
                TotalSum = cart.TotalSum,
                Discount = cart.Discount?.AsDto()
            };
        public static CheckoutCartDto AsDto(this CheckoutCart cart)
            => new()
            {
                Id = cart.Id,
                Customer = cart.Customer.AsDto(),
                Payment = cart.Payment?.AsDto(),
                Shipment = cart.Shipment?.AsDto(),
                IsPaid = cart.IsPaid,
                Products = cart.Products.Select(p => p.AsDto()),
                TotalSum = cart.TotalSum,
                AdditionalInformation = cart.AdditionalInformation,
                Discount = cart.Discount?.AsDto()
            };
        public static CartProductDto AsDto(this CartProduct product)
            => new()
            {
                Id = product.Id,
                ProductId = product.ProductId,
                SKU = product.Product.SKU,
                Name = product.Product.Name,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                ImagePathUrl = product.Product.ImagePathUrl,
                Quantity = product.Quantity
            };
        public static CheckoutStripeSessionDto AsDto(this Session session)
            => new()
            {
                SessionId = session.Id,
                CheckoutUrl = session.Url
            };
        public static ShipmentDto AsDto(this Shipment shipment)
            => new()
            {
                Country = shipment.Country,
                City = shipment.City,
                PostalCode = shipment.PostalCode,
                StreetName = shipment.StreetName,
                StreetNumber = shipment.StreetNumber,
                AparmentNumber = shipment.AparmentNumber
            };
        public static CustomerDto AsDto(this Customer customer)
            => new()
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        public static DiscountDto AsDto(this Discount discount)
            => new()
            {
                Code = discount.Code,
                Type = discount.Type.ToString(),
                Value = discount.Value,
                ExpiresAt = discount.ExpiresAt
            };
        public static PaymentDto AsDto(this Payment payment)
            => new()
            {
                PaymentMethod = payment.PaymentMethod.ToString(),
                IsActived = payment.IsActive
            };
    }
}
