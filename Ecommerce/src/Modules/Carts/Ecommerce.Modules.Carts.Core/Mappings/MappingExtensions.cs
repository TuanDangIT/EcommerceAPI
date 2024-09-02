using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                CustomerId = cart.CustomerId,
                Payment = cart.Payment,
                Shipment = cart.Shipment,
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
                PaymentMethod = payment.PaymentMethod,
            };
    }
}
