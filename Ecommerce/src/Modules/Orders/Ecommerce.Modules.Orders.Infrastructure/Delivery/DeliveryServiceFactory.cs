using Ecommerce.Modules.Orders.Application.Orders.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery
{
    public class DeliveryServiceFactory : IDeliveryServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DeliveryServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDeliveryService GetDeliveryService(string shippingService)
        {
            return shippingService.ToUpper() switch
            {
                "INPOST" => _serviceProvider.GetRequiredService<InpostService>(),
                "DPD" => _serviceProvider.GetRequiredService<DPDService>(),
                _ => throw new NotSupportedException($"Shipping service '{shippingService}' is not supported.")
            };
        }
    }
}
