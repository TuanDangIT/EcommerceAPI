using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Shared.Abstractions.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
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

namespace Ecommerce.Modules.Discounts.Tests.Integration.Controllers
{
    public class CouponsControllerCreateNominalCouponTests : ControllerTests
    {
        private readonly string _controllerName = "Coupons";
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public CouponsControllerCreateNominalCouponTests(EcommerceTestApp ecommerceTestApp) : base(ecommerceTestApp)
        {
        }

        [Fact]
        public async Task CreateNominalCoupon_WithCorrectData_ShouldReturn201WithCouponId()
        {
            //Arrange
            var dto = new NominalCouponCreateDto()
            {
                Name = "Test",
                NominalValue = 5
            };

            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + _controllerName + "/nominal-coupons", dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var couponCreateApiResponse = JsonSerializer.Deserialize<ApiResponseTest<CreateCouponData>>(httpContent, _jsonSerializerOptions);
            couponCreateApiResponse.Should().NotBeNull();
            couponCreateApiResponse.Code.Should().Be(HttpStatusCode.Created);
            couponCreateApiResponse.Status.Should().Be("success");
            couponCreateApiResponse.Data.Should().NotBeNull();
            var coupon = await DiscountsDbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponCreateApiResponse.Data.Id);
            coupon.Should().NotBeNull();
        }

        [MemberData(nameof(GetIncorrectNominalCouponDto))]
        [Theory]
        public async Task CreateNominalCoupon_WithIncorrectValidationData_ShouldReturn400WithErrorMessage(NominalCouponCreateDto dto)
        {
            //Act
            Authorize();
            var httpResponse = await HttpClient.PostAsJsonAsync(BaseEndpoint + _controllerName + "/nominal-coupons", dto);

            //Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var httpContent = await httpResponse.Content.ReadAsStringAsync();
            var couponCreateValidationExceptionResponse = JsonSerializer.Deserialize<ValidationExceptionResponseTest>(httpContent, _jsonSerializerOptions);
            couponCreateValidationExceptionResponse.Should().NotBeNull();
            couponCreateValidationExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
            couponCreateValidationExceptionResponse.Title.Should().Be("One or more validation errors occurred.");
        }

        private void Authorize()
        {
            var jwt = AuthHelper.CreateToken(Guid.NewGuid().ToString(), "username", "Admin");
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
        public static IEnumerable<object[]> GetIncorrectNominalCouponDto()
        {
            yield return new object[]
            {
                 new NominalCouponCreateDto()
                {
                    Name = "T",
                    NominalValue = -5
                }
            };

            yield return new object[]
            {
                 new NominalCouponCreateDto()
                {
                    Name = "Correct name",
                    NominalValue = -5
                }
            };

            yield return new object[]
            {
                 new NominalCouponCreateDto()
                {
                    Name = "C",
                    NominalValue = 5
                }
            };
        }
    }
}
