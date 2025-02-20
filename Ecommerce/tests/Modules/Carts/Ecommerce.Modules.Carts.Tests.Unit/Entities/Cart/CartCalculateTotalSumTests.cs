﻿using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Unit.Entities.Cart
{
    public class CartCalculateTotalSumTests
    {
        private readonly static DateTime _now = new(2022, 02, 25);
        private readonly static DateTime _futureDate = new(2022, 02, 26);
        [Theory]
        [MemberData((nameof(GetProductsWithExpectedTotalSumsForAddProducts)))]
        public void AddProduct_ShouldCalculateCorrectTotalValue(IEnumerable<Product> products, decimal expectedTotalSum)
        {
            var cart = GetCart();
            foreach (var product in products)
            {
                cart.AddProduct(product, 1);
            }
            cart.TotalSum.Should().Be(expectedTotalSum);
        }
        [Fact]
        public void RemoveProduct_ShouldCalculateCorrectTotalValue()
        {
            var cart = GetCart();
            var product = new Product("sku", "product", decimal.One, int.MaxValue, "image");
            cart.AddProduct(product, 5);
            cart.RemoveProduct(product, 1);
            cart.TotalSum.Should().Be(4);
        }
        [Fact]
        public void SetProductQuantity_ShouldCalculateCorrectTotalValue()
        {
            var cart = GetCart();
            var product = new Product("sku", "product", decimal.One, int.MaxValue, "image");
            cart.AddProduct(product, 1);
            cart.SetProductQuantity(product, 5);
            cart.TotalSum.Should().Be(5);
        }
        [Theory]
        [MemberData(nameof(GetProductsWithExpectedTotalSumsForAddDiscount))]
        public void AddDiscount_ShouldCalculateCorrectTotalValue(IEnumerable<Product> products, Discount discount, decimal expectedTotalSum)
        {
            var cart = GetCart();
            foreach(var product in products)
            {
                cart.AddProduct(product, 1);
            }
            cart.AddDiscount(discount);
            cart.TotalSum.Should().Be(expectedTotalSum);
        }
        [Theory]
        [MemberData(nameof(GetProductsWithExceedingDiscountForAddDiscount))]
        public void AddDiscount_WithExceedingValue_ShouldFail(IEnumerable<Product> products, Discount discount)
        {
            var cart = GetCart();
            foreach (var product in products)
            {
                cart.AddProduct(product, 1);
            }
            var exception = Record.Exception(() => cart.AddDiscount(discount)); 
            exception.Should().BeOfType<CartCalculatedTotalBelowOrEqualZeroException>();
        }
        [Theory]
        [MemberData(nameof(GetProductsWithDiscountForRemoveDiscount))]
        public void RemoveDiscount_ShouldCalculateCorrectTotalValue(IEnumerable<Product> products, Discount discount, decimal expectedTotalSum)
        {
            var cart = GetCart();
            foreach (var product in products)
            {
                cart.AddProduct(product, 1);
            }
            cart.AddDiscount(discount);
            cart.RemoveDiscount();
            cart.TotalSum.Should().Be(expectedTotalSum);
        }
        public static IEnumerable<object[]> GetProductsWithExpectedTotalSumsForAddProducts()
        {
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                33
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku1", "product", 5.5m, int.MaxValue, "image"),
                    new Product("sku2", "product", 5.22m, int.MaxValue, "image")
                },
                10.72
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 1.11m, int.MaxValue, "image")
                },
                1.11
            };
        }
        public static IEnumerable<object[]> GetProductsWithExpectedTotalSumsForAddDiscount()
        {
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("code", "sku", 5, Guid.NewGuid(), _futureDate, _now),
                18
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product1", 5.5m, int.MaxValue, "image"),
                    new Product("sku2", "product2", 5.22m, int.MaxValue, "image")
                },
                new Discount("code", "sku", 5, Guid.NewGuid(), _futureDate, _now),
                5.72
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 1.11m, int.MaxValue, "image")
                },
                new Discount("sku", DiscountType.NominalDiscount, 1, _futureDate, "promotionCodeId", _now),
                0.11
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.NominalDiscount, 20, _futureDate, "promotionCodeId", _now),
                13
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku1", "product", 5.5m, int.MaxValue, "image"),
                    new Product("sku2", "product", 5.22m, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.NominalDiscount, 10, _futureDate, "promotionCodeId", _now),
                0.72
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.PercentageDiscount, 0.5m, _futureDate, "promotionCodeId", _now),
                16.5
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku1", "product", 5.5m, int.MaxValue, "image"),
                    new Product("sku2", "product", 5.22m, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.PercentageDiscount, 0.5m, _futureDate, "promotionCodeId", _now),
                5.36
            };
        }
        public static IEnumerable<object[]> GetProductsWithExceedingDiscountForAddDiscount()
        {
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.NominalDiscount, 40, _futureDate, "promotionCodeId", _now)
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku1", "product", 5.4m, int.MaxValue, "image"),
                    new Product("sku2", "product", 5.22m, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.NominalDiscount, 10.72m, _futureDate, "promotionCodeId", _now)
            };
        }
        public static IEnumerable<object[]> GetProductsWithDiscountForRemoveDiscount()
        {
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku1", "product", 5.5m, int.MaxValue, "image"),
                    new Product("sku2", "product", 5.22m, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.NominalDiscount, 5, _futureDate, "promotionCodeId", _now),
                10.72
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("code", DiscountType.PercentageDiscount, 0.5m, _futureDate, "promotionCodeId", _now),
                33
            };
            yield return new object[]
            {
                new List<Product>()
                {
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image"),
                    new Product("sku", "product", 11, int.MaxValue, "image")
                },
                new Discount("sku", DiscountType.NominalDiscount, 5, _futureDate, "promotionCodeId", _now),
                33
            };
        }
        private Core.Entities.Cart GetCart()
            => new Core.Entities.Cart();
    }
}
