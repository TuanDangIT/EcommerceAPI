using Ecommerce.Shared.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using static Sieve.Extensions.MethodInfoExtended;

namespace Ecommerce.Modules.Carts.Tests.Integration.Controllers
{
    public class CartsControllerCreateCartTests : ControllerTests
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

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
            var cartCreatApiResponse = JsonSerializer.Deserialize<ApiResponseTest<CreateCartData>>(httpContent, _jsonSerializerOptions);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            cartCreatApiResponse.Should().NotBeNull();
            cartCreatApiResponse.Code.Should().Be(HttpStatusCode.Created);
            cartCreatApiResponse.Status.Should().Be("success");
            cartCreatApiResponse.Data.Should().NotBeNull();
            var cart = await CartsDbContext.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cartCreatApiResponse.Data.Id);
            cart.Should().NotBeNull();  
        }
    }
}
