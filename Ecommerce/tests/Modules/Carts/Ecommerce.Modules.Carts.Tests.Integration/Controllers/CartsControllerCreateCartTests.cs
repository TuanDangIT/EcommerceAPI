using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Sieve.Extensions.MethodInfoExtended;

namespace Ecommerce.Modules.Carts.Tests.Integration.Controllers
{
    public class CartsControllerCreateCartTests : ControllerTests
    {
        public CartsControllerCreateCartTests(EcommerceTestApp ecommerceTestApp) : base(ecommerceTestApp)
        {
        }

        [Fact]
        public async Task CreateCart_ShouldReturn201StatusAndId()
        {
            //Act
            var httpResponse = await HttpClient.PostAsync(BaseEndpoint, null);

            //Assert
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartCreatApiResponse = JsonConvert.DeserializeObject<ApiResponseTest<CreateCartData>>(httpContent);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            cartCreatApiResponse.Should().NotBeNull();
            cartCreatApiResponse.Code.Should().Be(HttpStatusCode.Created);
            cartCreatApiResponse.Status.Should().Be("success");
            cartCreatApiResponse.Data.Should().NotBeNull();
            var cart = await CartsDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartCreatApiResponse.Data.Id);
            cart.Should().NotBeNull();  
        }
    }
}
