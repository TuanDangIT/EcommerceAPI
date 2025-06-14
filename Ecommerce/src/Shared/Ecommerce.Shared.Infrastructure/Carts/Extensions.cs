using Ecommerce.Shared.Infrastructure.Delivery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Carts
{
    internal static class Extensions
    {
        private const string _cartOptionsSectionName = "Cart";
        public static IServiceCollection AddCartOptions(this IServiceCollection services)
        {
            var cartOptions = services.GetOptions<CartOptions>(_cartOptionsSectionName);
            services.AddSingleton(cartOptions);
            return services;
        }
    }
}
