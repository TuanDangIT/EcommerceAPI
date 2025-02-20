using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Shipment : AggregateRoot<int>
    {
        public string? TrackingNumber { get; private set; }
        public bool HasTrackingNumber => TrackingNumber is not null;
        public string? LabelId { get; private set; }
        public DateTime? LabelCreatedAt { get; private set; }
        public Receiver Receiver { get; private set; } = default!;
        public List<Parcel>? Parcels { get; private set; } = [];
        public Insurance? Insurance { get; private set; }
        public string Service { get; private set; } = "inpost_courier_standard";
        public Order Order { get; private set; } = default!;
        public Guid OrderId { get; private set; }
        public Shipment(Receiver receiver, IEnumerable<Parcel> parcels, decimal totalSum)
        {
            Receiver = receiver;
            Insurance = new Insurance(totalSum.ToString("0.00"));
            Parcels = parcels.ToList();
        }
        private Shipment()
        {

        }
        public void SetParcels(IEnumerable<Parcel> parcels)
        {
            Parcels = parcels.ToList();
            IncrementVersion();
        }
        public void SetTrackingNumber(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            IncrementVersion();
        }
        public void SetLabelId(string labelId)
        {
            LabelId = labelId;
            IncrementVersion();
        }
        public void SetLabelCreatedAt(DateTime labelCreatedAt)
        {
            LabelCreatedAt = labelCreatedAt;
            IncrementVersion();
        }
    }
}
