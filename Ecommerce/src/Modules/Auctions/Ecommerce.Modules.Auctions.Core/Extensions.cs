using Ecommerce.Modules.Auctions.Core.DAL;
using Ecommerce.Modules.Auctions.Core.Services;
using Ecommerce.Modules.Auctions.Core.Sieve;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecommerce.Modules.Auctions.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPostgres<AuctionDbContext>();
            services.AddSingleton<IAuctionDbContext>(sp => sp.GetRequiredService<AuctionDbContext>());
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, AuctionsModuleSieveProcessor>();
            return services;
        }
    }
}
