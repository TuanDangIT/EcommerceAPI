using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.DAL;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddPostgres<OrdersDbContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
