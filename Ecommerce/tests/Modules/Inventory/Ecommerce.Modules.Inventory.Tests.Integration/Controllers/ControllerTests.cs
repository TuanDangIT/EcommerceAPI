using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Inventory.Infrastructure.DAL;
using Ecommerce.Shared.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Integration.Controllers
{
    public class ControllerTests : IClassFixture<EcommerceTestApp>
    {
        internal readonly InventoryDbContext InventoryDbContext;
        protected readonly HttpClient HttpClient;
        protected readonly string BaseEndpoint = "/api/v1/inventory-module/";
        private readonly EcommerceTestApp _ecommerceTestApp;

        public ControllerTests(EcommerceTestApp ecommerceTestApp)
        {
            _ecommerceTestApp = ecommerceTestApp;
            HttpClient = _ecommerceTestApp.CreateClient();
            var scope = _ecommerceTestApp.Services.CreateScope();
            InventoryDbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            if (InventoryDbContext.Database.GetPendingMigrations().Any())
            {
                InventoryDbContext.Database.Migrate();
            }
        }

        protected void Authorize()
        {
            var jwt = AuthHelper.CreateToken(Guid.NewGuid().ToString(), "username", "Admin");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }
}
