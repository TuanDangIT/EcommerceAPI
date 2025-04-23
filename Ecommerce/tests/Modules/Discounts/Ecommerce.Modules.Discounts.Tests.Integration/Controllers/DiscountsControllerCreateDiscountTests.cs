using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Shared.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Ecommerce.Modules.Discounts.Tests.Integration.Controllers
{
    public class DiscountsControllerCreateDiscountTests : ControllerTests
    {
        protected new readonly string BaseEndpoint = "/api/v1/discounts-module/coupons/";
        private readonly string _controllerName = "Discounts";
        private readonly FakeTimeProvider _fakeTimeProvider;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public DiscountsControllerCreateDiscountTests(EcommerceTestApp ecommerceTestApp) : base(ecommerceTestApp)
        {
            _fakeTimeProvider = new FakeTimeProvider();
            _fakeTimeProvider.SetUtcNow(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        }

        [Fact]
        public async Task CreateDiscount_WithCorrectData_ShouldReturn201AndDiscountId()
        {
            //Arrange
            var couponId = await SeedCoupon();
            var dto = new DiscountCreateDto()
            {
                Code = "code",
                ExpiresDate = _fakeTimeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(2),
                RequiredCartTotalValue = 0
            };

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + couponId + "/" + _controllerName, dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var discountCreateApiResponse = JsonSerializer.Deserialize<ApiResponseTest<CreateDiscountData>>(httpContent, _jsonSerializerOptions);
            discountCreateApiResponse.Should().NotBeNull();
            discountCreateApiResponse.Code.Should().Be(HttpStatusCode.Created);
            discountCreateApiResponse.Status.Should().Be("success");
            discountCreateApiResponse.Data.Should().NotBeNull();
            var coupon = await DiscountsDbContext.Coupons
                .Include(c => c.Discounts)
                .FirstOrDefaultAsync(c => c.Id == couponId) ?? throw new NullReferenceException();
            var discount = coupon.Discounts.SingleOrDefault(d => d.Id == discountCreateApiResponse.Data.Id);
            discount.Should().NotBeNull();
        }

        [MemberData(nameof(GetIncorrectDiscountDto))]
        [Theory]
        public async Task CreateDiscount_WithIncorrectValidationData_ShouldReturn400WithErrorMessage(DiscountCreateDto dto)
        {
            //Arrange
            var couponId = await SeedCoupon();

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + couponId + "/" + _controllerName, dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var discountCreateValidationExceptionResponse = JsonSerializer.Deserialize<ValidationExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            discountCreateValidationExceptionResponse.Should().NotBeNull();
            discountCreateValidationExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            discountCreateValidationExceptionResponse.Title.Should().Be("One or more validation errors occurred.");
        }

        [Fact]
        public async Task CreateDiscount_WithDuplicatedDiscountCode_ShouldReturn400WithErrorMessage()
        {
            //Arrange
            var couponId = await SeedCoupon();
            await SeedDiscount(couponId);
            var dto = new DiscountCreateDto()
            {
                Code = "some-code",
                ExpiresDate = _fakeTimeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(2),
                RequiredCartTotalValue = 0
            };

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + couponId + "/" + _controllerName, dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var discountCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            discountCreateExceptionResponse.Should().NotBeNull();
            discountCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            discountCreateExceptionResponse.Title.Should().Be("An exception occurred.");
            discountCreateExceptionResponse.Detail.Should().Be($"Code: {dto.Code} is already in use.");
        }

        [Fact]
        public async Task CreateDiscount_WithNotExistingCouponId_ShouldReturn400WithErrorMessage()
        {
            //Arrange
            var couponId = Random.Shared.Next(100, 200);
            await SeedCoupon();
            var dto = new DiscountCreateDto()
            {
                Code = "code",
                ExpiresDate = _fakeTimeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(2),
                RequiredCartTotalValue = 0
            };

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + couponId + "/" + _controllerName, dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var discountCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            discountCreateExceptionResponse.Should().NotBeNull();
            discountCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            discountCreateExceptionResponse.Title.Should().Be("An exception occurred.");
            discountCreateExceptionResponse.Detail.Should().Be($"Coupon: {couponId} was not found.");
        }

        private async Task<int> SeedCoupon()
        {
            var coupon = new NominalCoupon("code", 5, "stripe-coupon-id");
            var couponEntry = await DiscountsDbContext.Coupons.AddAsync(coupon);
            await DiscountsDbContext.SaveChangesAsync();
            return couponEntry.Entity.Id;
        }

        private async Task SeedDiscount(int couponId)
        {
            var discount = new Discount("some-code", "stripe-promotion-id", 0, _fakeTimeProvider.GetUtcNow().UtcDateTime + TimeSpan.FromDays(2), 
                _fakeTimeProvider.GetUtcNow().UtcDateTime);
            var coupon = await DiscountsDbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponId)
                ?? throw new NullReferenceException();
            coupon.AddDiscount(discount);
            await DiscountsDbContext.SaveChangesAsync();
        }

        private void Authorize()
        {
            var jwt = AuthHelper.CreateToken(Guid.NewGuid().ToString(), "username", "Admin");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public static IEnumerable<object[]> GetIncorrectDiscountDto()
        {
            var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));

            yield return new object[]
            {
                 new DiscountCreateDto()
                {
                    Code = "c",
                    ExpiresDate = fakeTimeProvider.GetUtcNow().UtcDateTime - TimeSpan.FromDays(2),
                    RequiredCartTotalValue = -5
                }
            };

            yield return new object[]
            {
                 new DiscountCreateDto()
                {
                    
                }
            };

            yield return new object[]
            {
                 new DiscountCreateDto()
                {
                    Code = "code",
                    ExpiresDate = fakeTimeProvider.GetUtcNow().UtcDateTime - TimeSpan.FromDays(2)
                }
            };

            yield return new object[]
            {
                 new DiscountCreateDto()
                {
                    Code = "code",
                    RequiredCartTotalValue = -5
                }
            };
        }
    }
}
