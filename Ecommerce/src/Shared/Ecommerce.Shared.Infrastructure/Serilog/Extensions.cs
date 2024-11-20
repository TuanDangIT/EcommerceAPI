using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Serilog
{
    internal static class Extensions
    {
        public static ConfigureHostBuilder ConfigureSerilog(this ConfigureHostBuilder host)
        {
            host.UseSerilog((context, configuration) =>
            {
                configuration
                    .WriteTo
                    .Console();
                if (context.HostingEnvironment.IsDevelopment())
                {
                    configuration
                        .MinimumLevel
                        .Information();
                }
                else
                {
                    configuration
                        .MinimumLevel
                        .Error();
                }
            });
            return host;
        }
    }
}

