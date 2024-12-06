using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.Services;
using Ecommerce.Modules.Discounts.Core.Services.Externals;
using Ecommerce.Modules.Discounts.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core
{
    public static class Extensions
    {
        private const string _sieveSectionName = "Sieve";
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDiscountDbContext>(sp =>
            {
                return sp.GetRequiredService<DiscountsDbContext>();
            });
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IPaymentProcessorService, PaymentProcessorService>();    
            services.AddPostgres<DiscountsDbContext>();
            services.Configure<SieveOptions>(configuration.GetSection(_sieveSectionName));
            services.AddScoped<ISieveProcessor, DiscountsModuleSieveProcessor>();
            return services;
        }
    }
}
