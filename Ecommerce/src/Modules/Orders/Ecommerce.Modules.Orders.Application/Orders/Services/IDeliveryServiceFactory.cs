using Ecommerce.Modules.Orders.Application.Orders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    public interface IDeliveryServiceFactory
    {
        IDeliveryService GetDeliveryService(string shippingService);
    }

}
