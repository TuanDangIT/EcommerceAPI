using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ecommerce.Modules.Inventory.Tests.Integration.Controllers
{
    public class InventoryControllerCreateProductTests : ControllerTests
    {
        private readonly string _controllerName = "Products";
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public InventoryControllerCreateProductTests(EcommerceTestApp ecommerceTestApp, ITestOutputHelper testOutputHelper) : base(ecommerceTestApp)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CreateProduct_WithCorrectData_ShouldReturn201AndId()
        {
            //Arrange
            var (categoryId, manufacturerId, parameterId) = await Seed();
            var command = new CreateProduct();
            using var formContent = new MultipartFormDataContent
            {
                { new StringContent("12345678"), nameof(command.SKU) },
                { new StringContent("name"), nameof(command.Name) },
                { new StringContent(5.ToString()), nameof(command.Price) },
                { new StringContent(23.ToString()), nameof(command.VAT) },
                { new StringContent("description"), nameof(command.Description) },
                { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
                { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
            };
            var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
            {
                ParameterId = parameterId,
                Value = "value"
            });
            formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));
            var imageStream = new MemoryStream();
            var imageContent = new StreamContent(imageStream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            formContent.Add(imageContent, "image", "image.jpg");

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var productCreateApiResponse = JsonSerializer.Deserialize<ApiResponseTest<CreateProductData>>(httpContent, _jsonSerializerOptions);
            productCreateApiResponse.Should().NotBeNull();
            productCreateApiResponse.Code.Should().Be(HttpStatusCode.Created);
            productCreateApiResponse.Status.Should().Be("success");
            productCreateApiResponse.Data.Should().NotBeNull();
            var product = await InventoryDbContext.Products.FirstOrDefaultAsync(c => c.Id == productCreateApiResponse.Data.Id);
            product.Should().NotBeNull();

        }

        [Fact]
        public async Task CreateProduct_WithNotExistingCategory_ShouldReturn400AndErrorMessage()
        {
            //Arrange
            var (_, manufacturerId, parameterId) = await Seed();
            var categoryId = Guid.NewGuid();
            var command = new CreateProduct();
            using var formContent = new MultipartFormDataContent
            {
                { new StringContent("12345678"), nameof(command.SKU) },
                { new StringContent("name"), nameof(command.Name) },
                { new StringContent(5.ToString()), nameof(command.Price) },
                { new StringContent(23.ToString()), nameof(command.VAT) },
                { new StringContent("description"), nameof(command.Description) },
                { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
                { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
            };
            var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
            {
                ParameterId = parameterId,
                Value = "value"
            });
            formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);
            
            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            productCreateExceptionResponse.Should().NotBeNull();
            productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
            productCreateExceptionResponse.Detail.Should().Be($"Category: {categoryId} was not found.");
        }

        [Fact]
        public async Task CreateProduct_WithNotExistingManufacturer_ShouldReturn400AndErrorMessage()
        {
            //Arrange
            var (categoryId, _, parameterId) = await Seed();
            var manufacturerId = Guid.NewGuid();
            var command = new CreateProduct();
            using var formContent = new MultipartFormDataContent
            {
                { new StringContent("12345678"), nameof(command.SKU) },
                { new StringContent("name"), nameof(command.Name) },
                { new StringContent(5.ToString()), nameof(command.Price) },
                { new StringContent(23.ToString()), nameof(command.VAT) },
                { new StringContent("description"), nameof(command.Description) },
                { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
                { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
            };
            var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
            {
                ParameterId = parameterId,
                Value = "value"
            });
            formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            productCreateExceptionResponse.Should().NotBeNull();
            productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
            productCreateExceptionResponse.Detail.Should().Be($"Manufacturer: {manufacturerId} was not found.");
        }

        [Fact]
        public async Task CreateProduct_WithNotExistingParameter_ShouldReturn400AndErrorMessage()
        {
            //Arrange
            var (categoryId, manufacturerId, _) = await Seed();
            var parameterId = Guid.NewGuid();
            var command = new CreateProduct();
            using var formContent = new MultipartFormDataContent
            {
                { new StringContent("12345678"), nameof(command.SKU) },
                { new StringContent("name"), nameof(command.Name) },
                { new StringContent(5.ToString()), nameof(command.Price) },
                { new StringContent(23.ToString()), nameof(command.VAT) },
                { new StringContent("description"), nameof(command.Description) },
                { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
                { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
            };
            var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
            {
                ParameterId = parameterId,
                Value = "value"
            });
            formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            productCreateExceptionResponse.Should().NotBeNull();
            productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
            productCreateExceptionResponse.Detail.Should().Be($"Parameter: {parameterId} was not found.");
        }

        public async Task<(Guid CategoryId, Guid ManufacturerId, Guid ParameterId)> Seed()
        {
            var category = new Category("category");
            var manufacturer = new Manufacturer("manufacturer");
            var parameter = new Parameter("parameter");
            var categoryEntry = await InventoryDbContext.AddAsync(category);
            var parameterEntry = await InventoryDbContext.AddAsync(parameter);
            var manufacturerEntry = await InventoryDbContext.AddAsync(manufacturer);
            await InventoryDbContext.SaveChangesAsync();
            return (categoryEntry.Entity.Id, manufacturerEntry.Entity.Id, parameterEntry.Entity.Id);
        }
    }
}
