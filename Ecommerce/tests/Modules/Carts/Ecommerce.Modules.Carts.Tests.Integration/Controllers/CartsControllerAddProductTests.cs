using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Integration.Controllers
{
    public class CartsControllerAddProductTests : ControllerTests
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public CartsControllerAddProductTests(EcommerceTestApp ecommerceTestApp) : base(ecommerceTestApp)
        {
        }

        [Fact]
        public async Task AddProductToCart_WithCorrectData_ShouldReturn200AndAddProduct()
        {
            //Arange
            int quantity = 10;
            var (cartId, productId) = await SeedCartAndProduct(quantity);

            //Act
            var addQuantity = 5;
            var httpResponse = await HttpClient.PatchAsync(BaseEndpoint + $"/{cartId}/products/{productId}" + $"?quantity={addQuantity}", null);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var cart = await CartsDbContext.Carts
                .AsNoTracking()
                .Include(c => c.Products)
                .ThenInclude(cp => cp.Product)
                .FirstAsync(c => c.Id == cartId);
            cart.Products.Should().NotBeEmpty();
            cart.TotalSum.Should().Be(5);
            var cartProduct = cart.Products.SingleOrDefault(cp => cp.Product.Id == productId);
            cartProduct.Should().NotBeNull();
            cartProduct.Quantity.Should().Be(5);
            var product = cartProduct.Product;
            product.Id.Should().Be(productId);
            product.Quantity.Should().Be(quantity - addQuantity);
        }

        [Fact]
        public async Task AddProductToCart_WithIncorrectQuantityExceedingProductQuantity_ShouldReturn400WithBadRequestMessage()
        {
            //Arange
            var quantity = 2;
            var (cartId, productId) = await SeedCartAndProduct(quantity);

            //Act
            var addQuantity = 5;
            var httpResponse = await HttpClient.PatchAsync(BaseEndpoint + $"/{cartId}/products/{productId}" + $"?quantity={addQuantity}", null);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartAddProductExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            cartAddProductExceptionResponse.Should().NotBeNull();
            cartAddProductExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            cartAddProductExceptionResponse.Title.Should().Be("An exception occurred.");
            cartAddProductExceptionResponse.Detail.Should().Be($"Product: {productId} is out of stock.");
            var cart = await CartsDbContext.Carts
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstAsync(c => c.Id == cartId);
            cart.Products.Should().BeEmpty();
        }

        [Fact]
        public async Task AddProductToCart_WithIncorrectCartId_ShouldReturn400WithBadRequestMessage()
        {
            //Arange
            var (_, productId) = await SeedCartAndProduct(10);
            var cartId = Guid.NewGuid();

            //Act
            var httpResponse = await HttpClient.PatchAsync(BaseEndpoint + $"/{cartId}/products/{productId}" + $"?quantity={5}", null);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartAddProductExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            cartAddProductExceptionResponse.Should().NotBeNull();
            cartAddProductExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            cartAddProductExceptionResponse.Title.Should().Be("An exception occurred.");
            cartAddProductExceptionResponse.Detail.Should().Be($"Cart: {cartId} was not found.");
        }

        [Fact]
        public async Task AddProductToCart_WithIncorrectProductId_ShouldReturn400WithBadRequestMessage()
        {
            //Arange
            var (cartId, _) = await SeedCartAndProduct(10);
            var productId = Guid.NewGuid();

            //Act
            var httpResponse = await HttpClient.PatchAsync(BaseEndpoint + $"/{cartId}/products/{productId}" + $"?quantity={5}", null);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var cartAddProductExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            cartAddProductExceptionResponse.Should().NotBeNull();
            cartAddProductExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            cartAddProductExceptionResponse.Title.Should().Be("An exception occurred.");
            cartAddProductExceptionResponse.Detail.Should().Be($"Product with ID: {productId} was not found.");
        }

        public async Task<(Guid CartId, Guid ProductId)> SeedCartAndProduct(int quantity)
        {
            var cart = new Cart();
            var product = new Product("sku", "name", 1, quantity, "image");
            var productEntry = await CartsDbContext.AddAsync(product);
            var cartEntry = await CartsDbContext.AddAsync(cart);
            await CartsDbContext.SaveChangesAsync();
            return (cartEntry.Entity.Id, productEntry.Entity.Id);
        }
    }
}
