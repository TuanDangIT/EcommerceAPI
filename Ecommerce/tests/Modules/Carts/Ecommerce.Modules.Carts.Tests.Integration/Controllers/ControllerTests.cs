using Ecommerce.Modules.Carts.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Integration.Controllers
{
    public class ControllerTests : IClassFixture<EcommerceTestApp>
    {
        internal readonly CartsDbContext CartsDbContext;
        protected readonly HttpClient HttpClient;
        protected readonly string BaseEndpoint = "/api/v1/carts-module/Carts";
        private readonly EcommerceTestApp _ecommerceTestApp;
        //private readonly IServiceScope _scope;

        public ControllerTests(EcommerceTestApp ecommerceTestApp)
        {
            _ecommerceTestApp = ecommerceTestApp;
            HttpClient = _ecommerceTestApp.CreateClient();
            var scope = _ecommerceTestApp.Services.CreateScope();
            CartsDbContext = scope.ServiceProvider.GetRequiredService<CartsDbContext>();
            if (CartsDbContext.Database.GetPendingMigrations().Any())
            {
                CartsDbContext.Database.Migrate();
            }
        }
    }
}
