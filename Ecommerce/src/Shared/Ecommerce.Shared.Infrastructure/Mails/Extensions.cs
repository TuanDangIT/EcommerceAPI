using Ecommerce.Shared.Infrastructure.Delivery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Mails
{
    internal static class Extensions
    {
        private const string _mailOptionsSectionName = "Mail";
        public static IServiceCollection AddMails(this IServiceCollection services)
        {
            var mailOptions = services.GetOptions<MailOptions>(_mailOptionsSectionName);
            services.AddSingleton(mailOptions);
            return services;
        }
    }
}
