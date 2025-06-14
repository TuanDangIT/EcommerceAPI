using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Delivery
{
    internal static class Extensions
    {
        private const string _inPostOptionsSectionName = "InPost";
        private const string _dpdOptionsSectionName = "DPD";
        private const string _organizationSectionName = "Organization"; 
        public static IServiceCollection AddDelivery(this IServiceCollection services)
        {
            var inPostOptions = services.GetOptions<InPostOptions>(_inPostOptionsSectionName);
            services.AddSingleton(inPostOptions);
            var dpdOptions = services.GetOptions<DPDOptions>(_dpdOptionsSectionName);
            services.AddSingleton(dpdOptions);
            var organizationDetails = services.GetOptions<OrganizationDetails>(_organizationSectionName);
            services.AddSingleton(organizationDetails);
            return services;
        }
    }
}
