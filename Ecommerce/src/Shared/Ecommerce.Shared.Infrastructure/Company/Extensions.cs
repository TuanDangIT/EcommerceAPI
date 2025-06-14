using Ecommerce.Shared.Infrastructure.Delivery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Company
{
    internal static class Extensions
    {
        private const string _companyOptionsSectionName = "Company";
        public static IServiceCollection AddCompanyDetails(this IServiceCollection services)
        {
            var companyOptions = services.GetOptions<CompanyOptions>(_companyOptionsSectionName);
            services.AddSingleton(companyOptions);
            return services;
        }
    }
}
