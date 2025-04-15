using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Unit.Entities.Cart
{
    public class CartDiscountTests
    {
        private readonly DateTime _futureDate;
        private readonly DateTime _now;
        private readonly Discount _discountWithSKU;
        private readonly Discount _discountWithoutSKU;
        public CartDiscountTests()
        {
            _now = new DateTime(2022, 02, 25);
            _futureDate = new DateTime(2022, 02, 26);
            _discountWithSKU = new Discount("code", "sku", 10m, Guid.NewGuid(), _futureDate, _now);
            _discountWithoutSKU = new Discount("code", DiscountType.NominalDiscount, 5m, 0, _futureDate, "promotionCodeId", _now);
        }
        [Fact]
        public void AddIndividualDiscount_WithMatchingSKU_ShouldSucceed()
        {
            // Arrange
            var cart = GetCart("sku");
            var discount = _discountWithSKU;

            // Act
            cart.AddDiscount(discount);

            // Assert
            cart.Discount.Should().NotBeNull();
            cart.Discount.Should().Be(discount);
        }

        [Fact]
        public void AddIndividualDiscount_WithNoMatchingSKU_ShouldThrowCheckoutCartCannotApplyIndividualDiscountException()
        {
            // Arrange
            var cart = GetCart("otherSku");
            var discount = _discountWithSKU;

            // Act
            var exception = Record.Exception(() => cart.AddDiscount(discount));

            // Assert
            cart.Discount.Should().BeNull();
            exception.Should().BeOfType<CheckoutCartCannotApplyIndividualDiscountException>();
        }

        [Fact]
        public void AddDiscount_WithDiscountWithoutSKU_ShouldSucceed()
        {
            // Arrange
            var cart = GetCart("sku");
            var discount = _discountWithoutSKU;

            // Act
            cart.AddDiscount(discount);

            // Assert
            cart.Discount.Should().NotBeNull();
            cart.Discount.Should().Be(discount);
        }

        [Fact]
        public void RemoveDiscount_ShouldSucceed()
        {
            // Arrange
            var cart = GetCart("sku");
            var discount = _discountWithoutSKU;
            cart.AddDiscount(discount);

            // Act
            cart.RemoveDiscount();

            // Assert
            cart.Discount.Should().BeNull();
        }

        private Core.Entities.Cart GetCart(string sku)
        {
            var cart = new Core.Entities.Cart();
            var product = new Product(sku, "name", 50, null, "image");
            cart.AddProduct(product, 1);
            return cart;
        }
    }
}
