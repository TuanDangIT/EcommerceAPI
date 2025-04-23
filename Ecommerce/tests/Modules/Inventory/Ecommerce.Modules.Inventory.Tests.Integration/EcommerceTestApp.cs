using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Integration
{
    public class EcommerceTestApp : BaseTestApp
    {
        protected override void ConfigureTestServices(IServiceCollection services)
        {
            services.RemoveAll<IBlobStorageService>();
            services.AddSingleton<IBlobStorageService, FakeBlobStorageService>();
        }
    }
}
