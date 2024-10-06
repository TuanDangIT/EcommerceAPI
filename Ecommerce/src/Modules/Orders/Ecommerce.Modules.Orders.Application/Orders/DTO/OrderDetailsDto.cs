﻿using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public CustomerDto Customer { get; set; } = new();
        public decimal TotalSum { get; set; }
        public IEnumerable<ProductDto> Products { get; set; } = [];
        public ShipmentDto Shipment { get; set; } = new();
        public PaymentMethod Payment { get; set; }
        public OrderStatus Status { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? DiscountCode { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
