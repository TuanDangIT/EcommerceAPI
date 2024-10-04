using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.InPost
{
    internal static class Extensions
    {
        private const string _inPostOptionsSectionName = "InPost";
        public static IServiceCollection AddInpost(this IServiceCollection services)
        {
            var inPostOptions = services.GetOptions<InPostOptions>(_inPostOptionsSectionName);
            services.AddSingleton(inPostOptions);
            return services;
        }
    }
}
