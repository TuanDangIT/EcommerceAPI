using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events
{
    public sealed record class OfferRejected : IEvent
    {
        public int OfferId { get; private set; }    
        public Guid CustomerId { get; private set; }
        public string SKU { get; private set; } = string.Empty;
        public string ProductName { get; private set; } = string.Empty;
        public string? Code { get; private set; }
        public decimal OfferedPrice { get; private set; }   
        public decimal OldPrice { get; private set; }
        public OfferRejected(int offerId, Guid customerId, string sku, string productName, decimal offeredPrice, decimal oldPrice)
        {
            OfferId = offerId;
            CustomerId = customerId;
            SKU = sku;
            ProductName = productName;
            OfferedPrice = offeredPrice;
            OldPrice = oldPrice;
        }
        public OfferRejected(int offerId, Guid customerId, string code, string sku, string productName, decimal offeredPrice, decimal oldPrice)
        {
            OfferId = offerId;
            CustomerId = customerId;
            SKU = sku;
            ProductName = productName;
            Code = code;
            OfferedPrice = offeredPrice;
            OldPrice = oldPrice;
        }
    }
}
