using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static OrderBrowseDto AsBrowseDto(this Order order)
            => new()
            {
                Id = order.Id,
                CustomerId = order.Customer.UserId,
                FullName = order.Customer.FirstName + " " + order.Customer.LastName,
                Status = order.Status,
                TotalSum = order.TotalSum,
                OrderPlacedAt = order.OrderPlacedAt,
                UpdatedAt = order.UpdatedAt,
            };
        public static OrderDetailsDto AsDetailsDto(this Order order)
            => new()
            {
                Id = order.Id,
                Customer = order.Customer.AsDto(),
                TotalSum = order.TotalSum,
                Products = order.Products.Select(p => p.AsDto()),
                Shipment = order.Shipment.AsDto(),
                Payment = order.Payment,
                Status = order.Status,
                AdditionalInformation = order.AdditionalInformation,
                OrderPlacedAt = order.OrderPlacedAt,
                UpdatedAt = order.UpdatedAt
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
                CustomerId = customer.UserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        public static ReturnProductDto AsDto(this ReturnProduct product)
            => new()
            {
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                ImagePathUrl = product.ImagePathUrl
            };
        public static OrderShortenedDetailsDto AsShortenedDetailsDto(this Order order)
            => new()
            {
                OrderId = order.Id,
                TotalSum = order.TotalSum,
                Products = order.Products.Select(p => p.AsDto()),
                OrderPlacedAt = order.OrderPlacedAt
            };
        public static ComplaintBrowseDto AsBrowseDto(this Complaint complaint)
            => new()
            {
                Id = complaint.Id,
                CustomerFullName = complaint.Order.Customer.FirstName + " " + complaint.Order.Customer.LastName,
                OrderId = complaint.Order.Id,
                Title = complaint.Title,
                Status = complaint.Status,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
            };
        public static ComplaintDetailsDto AsDetailsDto(this Complaint complaint)
            => new()
            {
                Id = complaint.Id,
                Customer = complaint.Order.Customer.AsDto(),
                Order = complaint.Order.AsShortenedDetailsDto(),
                Title = complaint.Title,
                Description = complaint.Description,
                AdditionalNote = complaint.AdditionalNote,
                Decision = new DecisionDto()
                {
                    DecisionText = complaint.Decision!.DecisionText,
                    AdditionalInformation = complaint.Decision.AdditionalInformation
                },
                Status = complaint.Status,
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt
            };
        public static ReturnBrowseDto AsBrowseDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                OrderId = @return.Order.Id,
                ReasonForReturn = @return.ReasonForReturn,
                Status = @return.Status,
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };
        public static ReturnDetailsDto AsDetailsDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                Customer = @return.Order.Customer.AsDto(),
                Order = @return.Order.AsShortenedDetailsDto(),
                Products = @return.Products.Select(p => p.AsDto()),
                ReasonForReturn = @return.ReasonForReturn,
                AdditionalNote = @return.AdditionalNote,
                Status = @return.Status,
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };
    }
}
