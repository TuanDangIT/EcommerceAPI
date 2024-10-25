using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.RateLimiter
{
    internal static class Extensions
    {
        //To powinno być z appsettings?
        private const double _windowSpanInSeconds = 10;
        private const int _permitLimit = 30;
        private const string _globalPolicyName = "fixed-by-ip";
        //private const int _queueLimit = 1;
        public static IServiceCollection AddAspRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy(_globalPolicyName, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = _permitLimit,
                            Window = TimeSpan.FromMinutes(_windowSpanInSeconds),
                            //QueueLimit = 1,
                            //QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));

            });
            return services;
        }
        public static WebApplication UseAspRateLimiter(this WebApplication app)
        {
            app.UseRateLimiter();
            return app;
        }
    }
}
