using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Shared.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using System;
using Testcontainers.PostgreSql;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Integration
{
    public class EcommerceTestApp : BaseTestApp
    {
        protected override void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureTestServices(services);
        }
    }
}
