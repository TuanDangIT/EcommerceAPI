using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static OrderBrowseDto AsBrowseDto(this Order order)
            => new()
            {
                Id = order.Id,
                CustomerId = order.Customer.CustomerId,
                FullName = order.Customer.FirstName + " " + order.Customer.LastName,
                Status = order.Status,
                OrderPlacedAt = order.OrderPlacedAt
            };
        public static OrderDetailsDto AsDetailsDto(this Order order)
            => new()
            {
                Id = order.Id,
                Customer = order.Customer.AsDto(),
                Products = order.Products.Select(p => p.AsDto()),
                Shipment = order.Shipment.AsDto(),
                Payment = order.Payment,
                Status = order.Status,
                AdditionalInformation = order.AdditionalInformation,
                OrderPlacedAt = order.OrderPlacedAt
            };
        public static ShipmentDto AsDto(this Shipment shipment)
            => new()
            {
                City = shipment.City,
                PostalCode = shipment.PostalCode,
                StreetName = shipment.StreetName,
                StreetNumber = shipment.StreetNumber,
                AparmentNumber = shipment.ApartmentNumber
            };
        public static ProductDto AsDto(this Product product)
            => new()
            {
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                ImagePathUrl = product.ImagePathUrl
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
    }
}
