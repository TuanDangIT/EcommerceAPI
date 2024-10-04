using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Entities
{
    public class Shipment
    {
        public int Id { get; set; } 
        public string? TrackingNumber { get; set; }
        public string? LabelId { get; set; } 
        public Receiver Receiver { get; set; } = new();
        public List<Parcel>? Parcels { get; set; } = [];
        public Insurance? Insurance { get; set; }
        public string Service { get; set; } = "inpost_courier_standard";
        public Order Order { get; set; } = new();
        public Guid OrderId { get; set; }
        public Shipment()
        {
            
        }
        public Shipment(Receiver receiver, decimal totalSum)
        {
            Receiver = receiver;
            Insurance = new Insurance(totalSum.ToString("0.00"));
        }
        public void SetParcels(IEnumerable<Parcel> parcels)
            => Parcels = parcels.ToList();
        public void SetTrackingNumber(string trackingNumber)    
            => TrackingNumber = trackingNumber;
        public void SetLabelId(string labelId)
            => LabelId = labelId;   
    }
}
