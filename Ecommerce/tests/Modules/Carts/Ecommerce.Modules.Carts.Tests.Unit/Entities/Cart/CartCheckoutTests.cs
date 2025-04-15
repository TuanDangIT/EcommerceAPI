using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Carts.Core.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Unit.Entities.Cart
{
    public class CartCheckoutTests
    {
        [Fact]
        public void Checkout_WithProduct_ShouldReturnCheckoutCart()
        {
            // Arrange
            var cart = GetCart(true);

            // Act
            var checkoutCart = cart.Checkout(null);

            // Assert
            checkoutCart.Should().NotBeNull();
        }

        [Fact]
        public void Checkout_WithNoProduct_ShouldFail()
        {
            // Arrange
            var cart = GetCart(false);

            // Act
            var exception = Record.Exception(() => cart.Checkout(null));

            // Assert
            exception.Should().BeOfType<Core.Entities.Exceptions.CartCheckoutWithNoProductsException>();
        }

        [Fact]
        public void Checkout_ThatHasBeenAlreadyChecked_ShouldReturnOldCheckoutCart()
        {
            // Arrange
            var cart = GetCart(true);

            // Act
            var oldCheckoutCart = cart.Checkout(null);
            var newCheckoutCart = cart.Checkout(null);

            // Assert
            newCheckoutCart.Should().Be(oldCheckoutCart);
        }

        private Core.Entities.Cart GetCart(bool withProduct)
        {
            var cart = new Core.Entities.Cart();
            var product = new Product("sku", "product", decimal.One, int.MaxValue, "image");

            if (withProduct)
            {
                cart.AddProduct(product, 1);
            }

            return cart;
        }
    }
}
