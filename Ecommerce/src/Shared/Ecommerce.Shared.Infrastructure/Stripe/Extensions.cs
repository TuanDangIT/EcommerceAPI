using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Stripe
{
    internal static class Extensions
    {
        private const string _stripeOptionsSectionName = "Stripe";
        public static IServiceCollection AddStripe(this IServiceCollection services)
        {
            var stripeOptions = services.GetOptions<StripeOptions>(_stripeOptionsSectionName);
            services.AddSingleton(stripeOptions);
            return services;
        }
    }
}
