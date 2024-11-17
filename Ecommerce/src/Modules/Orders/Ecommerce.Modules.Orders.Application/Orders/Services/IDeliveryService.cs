using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    public interface IDeliveryService
    {
        Task<(int Id, string TrackingNumber)> CreateShipmentAsync(Shipment shipment);
        Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment);
    }
}
