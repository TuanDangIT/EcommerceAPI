//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecommerce.Modules.Carts.Core.Entities;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace Ecommerce.Modules.Carts.Tests.Unit.Entities.Cart
//{
//    public class CartCheckoutTests
//    {
//        [Fact]
//        public void Checkout_WithProduct_ShouldReturnCheckoutCart()
//        {
//            var cart = GetCart(true);
//            var checkoutCart = cart.Checkout(null);
//            checkoutCart.Should().NotBeNull();
//        }
//        [Fact]
//        public void Checkout_WithNoProduct_ShouldFail()
//        {
//            var cart = GetCart(false);
//            var exception = Record.Exception(() => cart.Checkout(null));
//            exception.Should().BeOfType<Core.Entities.Exceptions.CartCheckoutWithNoProductsException>();
//        }
//        [Fact]
//        public void Checkout_ThatHasBeenAlreadyChecked_ShouldReturnOldCheckoutCart()
//        {
//            var cart = GetCart(true);
//            var oldCheckoutCart = cart.Checkout(null);
//            var newCheckoutCart = cart.Checkout(null);
//            newCheckoutCart.Should().Be(oldCheckoutCart);
//        }
//        private Core.Entities.Cart GetCart(bool withProduct)
//        {
//            var cart = new Core.Entities.Cart();
//            var product = new Product("sku", "product", decimal.One, int.MaxValue, "image");
//            if (withProduct)
//            {
//                cart.AddProduct(product, 1);
//            }
//            return cart;
//        }
//    }
//}
