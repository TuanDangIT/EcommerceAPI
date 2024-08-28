using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    internal static class Extensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IContextFactory, ContextFactory>();
            services.AddTransient(sp => sp.GetRequiredService<IContextFactory>().Create());
            return services;
        }
    }
}
