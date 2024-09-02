using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecommerce.Modules.Carts.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddPostgres<CartsDbContext>();
            services.AddSingleton<ICartsDbContext>(sp =>
            {
                return sp.GetRequiredService<CartsDbContext>();
            });
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICheckoutCartService, CheckoutCartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
