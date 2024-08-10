using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddExceptionHandler<AppExceptionHandler>();

            return services;
        }
        public static WebApplication UseErrorHandling(this WebApplication app)
        {
            app.UseExceptionHandler(_ => { });
            return app;
        }
    }
}
