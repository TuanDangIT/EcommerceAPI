using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Inventory.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Integration.Controllers
{
    public class ControllerTests : IClassFixture<EcommerceTestApp>
    {
        internal readonly InventoryDbContext CartsDbContext;
        protected readonly HttpClient HttpClient;
        protected readonly string BaseEndpoint = "/api/v1/carts-module/Carts";
        private readonly EcommerceTestApp _ecommerceTestApp;

        public ControllerTests(EcommerceTestApp ecommerceTestApp)
        {
            _ecommerceTestApp = ecommerceTestApp;
            HttpClient = _ecommerceTestApp.CreateClient();
            var scope = _ecommerceTestApp.Services.CreateScope();
            CartsDbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            if (CartsDbContext.Database.GetPendingMigrations().Any())
            {
                CartsDbContext.Database.Migrate();
            }
        }
    }
}
