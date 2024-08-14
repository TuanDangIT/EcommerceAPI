using Azure.Storage.Blobs;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage
{
    internal static class Extensions
    {
        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
        {
            var options = services.GetOptions<BlobStorageOptions>("BlobAzureStorage");
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            services.AddSingleton(_ => new BlobServiceClient(options.ConnectionString));
            return services;
        }
    }
}
