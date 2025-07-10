using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Shared.Infrastructure.Delivery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery
{
    internal static class Extensions
    {
        public static IServiceCollection AddDelivery(this IServiceCollection services)
        {
            services.AddScoped<InpostService>();
            services.AddScoped<DPDService>();

            services.AddScoped<IDeliveryServiceFactory, DeliveryServiceFactory>();

            services.AddHttpClient<IDeliveryService, InpostService>("InPost", (sp, client) =>
            {
                var inPostOptions = sp.GetRequiredService<InPostOptions>();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer" + " " + inPostOptions.ApiKey);
                client.BaseAddress = new Uri(inPostOptions.BaseUrl);
            }).AddStandardResilienceHandler();
            services.AddHttpClient<IDeliveryService, DPDService>("DPD", (sp, client) =>
            {
                var dpdOptions = sp.GetRequiredService<DPDOptions>();
                client.DefaultRequestHeaders.Add("Authorization", "Basic" + " " + dpdOptions.ApiKey);
                client.DefaultRequestHeaders.Add("X-Dpd-Fid", dpdOptions.OrganizationFID.ToString());
                client.BaseAddress = new Uri(dpdOptions.BaseUrl);
            }).AddStandardResilienceHandler();
            return services;
        }

    }
}
