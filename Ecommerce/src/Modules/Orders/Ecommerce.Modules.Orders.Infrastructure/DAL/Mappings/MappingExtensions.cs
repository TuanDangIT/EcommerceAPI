using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using System.Runtime.CompilerServices;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static OrderBrowseDto AsBrowseDto(this Order order)
            => new()
            {
                Id = order.Id,
                CustomerId = order.Customer?.UserId,
                FullName = order.Customer?.FirstName + " " + order.Customer?.LastName,
                Status = order.Status.ToString(),
                TotalSum = order.TotalSum,
                PlacedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
            };

        public static OrderCustomerBrowseDto AsCustomerBrowseDto(this Order order)
            => new()
            {
                Id = order.Id,
                FullName = order.Customer?.FirstName + " " + order.Customer?.LastName,
                Status = order.Status.ToString(),
                TotalSum = order.TotalSum,
                PlacedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
            };

        public static OrderDetailsDto AsDetailsDto(this Order order, bool hasCustomerConstraint)
            => new()
            {
                Id = order.Id,
                Customer = order.Customer?.AsDto(),
                TotalSum = order.TotalSum,
                Products = order.Products.Select(p => p.AsDto()),
                Shipment = order.Shipments.Select(s => s.AsDetailsDto()),
                Payment = order.Payment.ToString(),
                Status = order.Status.ToString(),
                ClientAdditionalInformation = order.ClientAdditionalInformation,
                CompanyAdditionalInformation = !hasCustomerConstraint ? order.CompanyAdditionalInformation : null,
                Discount = order.Discount?.AsDto(),
                Return = order.Return?.AsOrderReturnDto(),
                Complaints = order.Complaints?.Select(c => c.AsOrderComplaintDto()),
                PlacedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };

        public static OrderComplaintDto AsOrderComplaintDto(this Complaint complaint)
            => new()
            {
                Id = complaint.Id,
                Title = complaint.Title,
                Status = complaint.Status.ToString(),
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
            };

        public static OrderReturnDto AsOrderReturnDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                Status = @return.Status.ToString(),
                NumberOfProductsReturned = @return.Products.Count(),
                TotalSum = @return.TotalSum,
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt,
            };

        public static DiscountDto AsDto(this Discount discount)
            => new()
            {
                Type = discount.Type.ToString(),
                Code = discount.Code,
                SKU = discount.SKU,
                Value = discount.Value
            };
        public static ProductDto AsDto(this Product product)
            => new()
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                ImagePathUrl = product.ImagePathUrl
            };
        public static CustomerDto AsDto(this Customer customer)
            => new()
            {
                Id = customer.Id,
                CustomerId = customer.UserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address.AsDto()
            };
        public static AddressDto AsDto(this Address address)
            => new()
            {
                Country = address.Country,
                City = address.City,
                BuildingNumber = address.BuildingNumber,
                PostCode = address.PostCode,
                Street = address.Street
            };
        public static ReturnProductDto AsDto(this ReturnProduct product)
            => new()
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                UnitPrice = product.UnitPrice,
                Quantity = product.Quantity,
                ImagePathUrl = product.ImagePathUrl,
                Status = product.Status.ToString()
            };
        public static OrderShortenedDetailsDto AsShortenedDetailsDto(this Order order)
            => new()
            {
                Id = order.Id,
                Customer = order.Customer!.AsDto(),
                TotalSum = order.TotalSum,
                Products = order.Products.Select(p => p.AsDto()),
                PlacedAt = order.CreatedAt
            };
        public static ComplaintBrowseDto AsBrowseDto(this Complaint complaint)
            => new()
            {
                Id = complaint.Id,
                CustomerFullName = complaint.Order.Customer!.FirstName + " " + complaint.Order.Customer!.LastName,
                OrderId = complaint.OrderId,
                Title = complaint.Title,
                Status = complaint.Status.ToString(),
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt,
            };
        public static ComplaintDetailsDto AsDetailsDto(this Complaint complaint, bool hasCustomerConstraint)
            => new()
            {
                Id = complaint.Id,
                //Customer = complaint.Order.Customer.AsDto(),
                Order = complaint.Order.AsShortenedDetailsDto(),
                Title = complaint.Title,
                Description = complaint.Description,
                AdditionalNote = !hasCustomerConstraint ? complaint.AdditionalNote : null,
                Decision = new DecisionApproveOrEditDto()
                {
                    DecisionText = complaint.Decision!.DecisionText,
                    AdditionalInformation = complaint.Decision.AdditionalInformation,
                    RefundAmount = complaint.Decision.RefundAmount
                },
                Status = complaint.Status.ToString(),
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt
            };
        public static ReturnBrowseDto AsBrowseDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                OrderId = @return.OrderId,
                CustomerFullName = @return.Order.Customer!.FirstName + " " + @return.Order.Customer.LastName,
                ReasonForReturn = @return.ReasonForReturn,
                Status = @return.Status.ToString(),
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };
        public static ReturnDetailsDto AsDetailsDto(this Return @return, bool hasCustomerConstraint)
            => new()
            {
                Id = @return.Id,
                Order = @return.Order.AsShortenedDetailsDto(),
                Products = @return.Products.Select(p => p.AsDto()),
                TotalSum = @return.TotalSum,
                TotalSumLeftToReturn = !hasCustomerConstraint ? @return.TotalSumLeftToReturn : null,
                ReasonForReturn = @return.ReasonForReturn,
                AdditionalNote = !hasCustomerConstraint ? @return.AdditionalNote : null,
                RejectReason = @return.RejectReason,
                Status = @return.Status.ToString(),
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };

        public static InvoiceBrowseDto AsBrowseDto(this Invoice invoice)
            => new()
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                CustomerFullName = invoice.Order.Customer!.FirstName + " " + invoice.Order.Customer!.LastName,
                InvoiceNo = invoice.InvoiceNo,
                CreatedAt = invoice.CreatedAt
            };

        public static ShipmentBrowseDto AsBrowseDto(this Shipment shipment)
            => new()
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                CreatedAt = (DateTime)shipment.LabelCreatedAt!,
                ShippingService = shipment.Service,
                TrackingNumber = shipment.TrackingNumber!
            };

        public static ShipmentDetailsDto AsDetailsDto(this Shipment shipping)
            => new()
            {
                Id = shipping.Id,
                TrackingNumber = shipping.TrackingNumber!,
                ShippingService = shipping.Service,
                CreatedAt = (DateTime)shipping.LabelCreatedAt!
            };
    }
}
