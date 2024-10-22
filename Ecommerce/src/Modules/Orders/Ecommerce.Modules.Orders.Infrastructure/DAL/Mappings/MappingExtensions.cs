﻿using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Invoices.DTO;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Shipping.DTO;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Invoices.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;

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
                Status = order.Status.ToString(),
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
                //Shipment = order.ShipmentDetails.AsDto(),
                Payment = order.Payment.ToString(),
                Status = order.Status.ToString(),
                ClientAdditionalInformation = order.ClientAdditionalInformation,
                CompanyAdditionalInformation = order.CompanyAdditionalInformation,
                DiscountCode = order.DiscountCode,
                OrderPlacedAt = order.OrderPlacedAt,
                UpdatedAt = order.UpdatedAt
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
                Customer = order.Customer.AsDto(),
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
                Status = complaint.Status.ToString(),
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
                    AdditionalInformation = complaint.Decision.AdditionalInformation,
                    RefundedAmount = complaint.Decision.RefundedAmount
                },
                Status = complaint.Status.ToString(),
                CreatedAt = complaint.CreatedAt,
                UpdatedAt = complaint.UpdatedAt
            };
        public static ReturnBrowseDto AsBrowseDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                OrderId = @return.Order.Id,
                ReasonForReturn = @return.ReasonForReturn,
                Status = @return.Status.ToString(),
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };
        public static ReturnDetailsDto AsDetailsDto(this Return @return)
            => new()
            {
                Id = @return.Id,
                Order = @return.Order.AsShortenedDetailsDto(),
                Products = @return.Products.Select(p => p.AsDto()),
                ReasonForReturn = @return.ReasonForReturn,
                AdditionalNote = @return.AdditionalNote,
                RejectReason = @return.RejectReason,
                Status = @return.Status.ToString(),
                CreatedAt = @return.CreatedAt,
                UpdatedAt = @return.UpdatedAt
            };
        public static InvoiceBrowseDto AsBrowseDto(this Invoice invoice)
            => new()
            {
                Id = invoice.Id,
                CustomerFullName = invoice.Order.Customer.FirstName + " " + invoice.Order.Customer.LastName,
                InvoiceNo = invoice.InvoiceNo,
                CreatedAt = invoice.CreatedAt
            };
        public static ShipmentBrowseDto AsBrowseDto(this Shipment shipment)
            => new()
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                LabelId = shipment.LabelId!,
                CreatedAt = (DateTime)shipment.LabelCreatedAt!,
                ShippingService = shipment.Service,
                TrackingNumber = shipment.TrackingNumber!
            };
    }
}
