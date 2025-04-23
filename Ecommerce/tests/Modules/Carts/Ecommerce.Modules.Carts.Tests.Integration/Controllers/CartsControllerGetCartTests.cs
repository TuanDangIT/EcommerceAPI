using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Tests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Integration.Controllers
{
    public class CartsControllerGetCartTests : ControllerTests
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public CartsControllerGetCartTests(EcommerceTestApp ecommerceTestApp) : base(ecommerceTestApp)
        {
        }

        [Fact]
        public async Task GetCart_ThatExistsWithCorrectGuid_ShouldReturn200AndCart()
        {
            //Arrange
            var cartId = await SeedCart();

            //Act
            var httpResponse = await HttpClient.GetAsync(BaseEndpoint + $"/{cartId}");

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartGetApiResponse = JsonSerializer.Deserialize<ApiResponseTest<GetCartData>>(httpContent, _jsonSerializerOptions);
            cartGetApiResponse.Should().NotBeNull();
            cartGetApiResponse.Code.Should().Be(HttpStatusCode.OK);
            cartGetApiResponse.Status.Should().Be("success");
            cartGetApiResponse.Data.Should().NotBeNull();
            cartGetApiResponse.Data.Id.Should().Be(cartId);
        }

        [Fact]
        public async Task GetCart_ThatDoesNotExistWithCorrectGuid_ShouldReturn404AndNotFoundMessage()
        {
            //Arrange
            var cartId = Guid.NewGuid();
            //Act
            var httpResponse = await HttpClient.GetAsync(BaseEndpoint + $"/{cartId}");

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartGetExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            cartGetExceptionResponse.Should().NotBeNull();
            cartGetExceptionResponse.Status.Should().Be(HttpStatusCode.NotFound);
            cartGetExceptionResponse.Title.Should().Be("An exception occurred.");
            cartGetExceptionResponse.Detail.Should().Be($"Cart: {cartId} was not found.");
        }

        [Fact]
        public async Task GetCart_IncorrectGuid_ShouldReturn404()
        {
            //Act
            var httpResponse = await HttpClient.GetAsync(BaseEndpoint + "/random-string");

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public async Task<Guid> SeedCart()
        {
            var cart = new Cart();
            var entry = await CartsDbContext.AddAsync(cart);
            await CartsDbContext.SaveChangesAsync();
            return entry.Entity.Id;
        }
    }
}
