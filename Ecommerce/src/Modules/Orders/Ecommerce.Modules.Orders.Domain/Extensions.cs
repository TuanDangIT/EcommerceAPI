using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<IOrderCancellationPolicy, OrderCancellationPolicy>();
            services.AddSingleton<IOrderReturnPolicy, OrderReturnPolicy>();
            return services;
        }
    }
}
