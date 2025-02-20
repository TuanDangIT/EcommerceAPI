using Ecommerce.Modules.Carts.Core.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Unit.Entities.Cart
{
    public class CartManageProductsTests
    {
        private readonly Product _product;
        public CartManageProductsTests()
        {
            _product = new Product("sku", "product", decimal.One, 10, "image");
        }
        [Fact]
        public void AddProduct_ThatDoesNotExistInCart_ShouldSucceed()
        {
            var cart = GetCart(false);
            cart.AddProduct(_product, 5);
            cart.Products.Should().NotBeEmpty();
            cart.Products.Should().NotBeNull();
            cart.Products.Should().HaveCount(1);
            cart.Products.SingleOrDefault(p => p.Product == _product).Should().NotBeNull();
        }
        [Fact]
        public void AddProduct_ThatDoesExistInCart_ShouldSucceed()
        {
            var cart = GetCart(true);
            cart.AddProduct(_product, 5);
            cart.Products.SingleOrDefault(p => p.Product == _product).Should().NotBeNull();
            cart.Products.Single(p => p.Product == _product).Quantity.Should().Be(6);
        }
        private Core.Entities.Cart GetCart(bool withProduct)
        {
            var cart = new Core.Entities.Cart();
            var product = _product;
            if (withProduct)
            {
                cart.AddProduct(product, 1);
            }
            return cart;
        }
    }
}
