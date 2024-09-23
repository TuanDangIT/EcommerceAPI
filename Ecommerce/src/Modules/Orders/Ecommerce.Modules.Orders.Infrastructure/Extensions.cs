using Ecommerce.Modules.Orders.Application.Stripe;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.DAL;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.Stripe;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddPostgres<OrdersDbContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReturnRepository, ReturnRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddSingleton<IStripeService, StripeService>();
            return services;
        }
    }
}
