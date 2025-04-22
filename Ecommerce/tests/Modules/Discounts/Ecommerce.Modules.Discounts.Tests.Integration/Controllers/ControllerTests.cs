using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Discounts.Tests.Integration.Controllers
{
    public class ControllerTests : IClassFixture<EcommerceTestApp>
    {
        internal readonly DiscountsDbContext DiscountsDbContext;
        protected readonly HttpClient HttpClient;
        protected readonly string BaseEndpoint = "/api/v1/discounts-module/";
        private readonly EcommerceTestApp _ecommerceTestApp;

        public ControllerTests(EcommerceTestApp ecommerceTestApp)
        {
            _ecommerceTestApp = ecommerceTestApp;
            HttpClient = _ecommerceTestApp.CreateClient();
            var scope = _ecommerceTestApp.Services.CreateScope();
            DiscountsDbContext = scope.ServiceProvider.GetRequiredService<DiscountsDbContext>();
            if (DiscountsDbContext.Database.GetPendingMigrations().Any())
            {
                DiscountsDbContext.Database.Migrate();
            }
        }
    }
}
