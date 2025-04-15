using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Unit.Entities.Product
{
    public class ProductPurchaseTests
    {
        [Theory]
        [MemberData(nameof(GetValidPurchaseTestCases))]
        public void PurchaseProduct_WithValidParameters_ShouldDeleteReservedQuantity(int? initialQuantity, int initialReserved, int purchaseQuantity, int expectedReserved)
        {
            // Arrange
            var product = CreateProduct(quantity: initialQuantity, reserved: initialReserved);

            // Act
            product.Purchase(purchaseQuantity);

            // Assert
            product.Reserved.Should().Be(expectedReserved);
            product.Quantity.Should().Be(initialQuantity);
        }

        [Theory]
        [MemberData(nameof(GetInvalidQuantityTestCases))]
        public void PurchaseProduct_WithIncorrectQuantityBelowOrEqualZero_ShouldThrowQuantityBelowOrEqualZeroException(int invalidQuantity)
        {
            // Arrange
            var product = CreateProduct(quantity: 10, reserved: 5);

            // Act
            Action act = () => product.Purchase(invalidQuantity);

            // Assert
            act.Should().Throw<QuantityBelowOrEqualZeroException>();
            product.Reserved.Should().Be(5);
            product.Quantity.Should().Be(10);
        }

        [Fact]
        public void PurchaseProduct_WithIncorrectReservationEqualZero_ShouldThrowCannotPurchaseProductBeforeReservingException()
        {
            // Arrange
            var product = CreateProduct(quantity: 10, reserved: 0);

            // Act
            Action act = () => product.Purchase(3);

            // Assert
            act.Should().Throw<CannotPurchaseProductBeforeReservingException>();
            product.Reserved.Should().Be(0);
            product.Quantity.Should().Be(10);
        }

        [Fact]
        public void PurchaseProduct_WithIncorrectQuantityHigherThanReservation_ShouldThrowProductReservedBelowZeroException()
        {
            // Arrange
            var product = CreateProduct(quantity: 10, reserved: 5);

            // Act
            Action act = () => product.Purchase(7);

            // Assert
            act.Should().Throw<ProductReservedBelowZeroException>();
            product.Reserved.Should().Be(5);
            product.Quantity.Should().Be(10);
        }

        [Fact]
        public void PurchaseProduct_WithCorrectQuantityAndNoQuantity_ShouldNotRemoveReservedQuantityAndSucceed()
        {
            // Arrange
            var product = CreateProduct(quantity: null, reserved: null);
            int purchaseQuantity = 3;

            // Act
            product.Purchase(purchaseQuantity);

            // Assert
            product.Reserved.Should().BeNull();
            product.Quantity.Should().BeNull();
            product.HasQuantity.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetValidPurchaseTestCases()
        {
            yield return new object[] { 10, 5, 3, 2 };         // Standard successful purchase
            yield return new object[] { 10, 5, 5, 0 };         // Purchase all reserved items
            yield return new object[] { 0, 5, 3, 2 };          // No quantity but valid reservation
            yield return new object[] { null!, 5, 3, 5 };       // Null quantity but valid reservation
        }

        public static IEnumerable<object[]> GetInvalidQuantityTestCases()
        {
            yield return new object[] { 0 };                   // Zero quantity
            yield return new object[] { -1 };                  // Negative quantity
            yield return new object[] { -5 };                  // Another negative quantity
            yield return new object[] { int.MinValue };        // Extreme negative quantity
        }

        private Domain.Inventory.Entities.Product CreateProduct(int? quantity, int? reserved)
        {
            return new Domain.Inventory.Entities.Product(
                sku: "TEST-SKU-123",
                name: "Test Product",
                price: 10.99m,
                vat: 23,
                description: "Test description",
                productParameters: new List<ProductParameter>(),
                manufacturer: null,
                category: null,
                images: new List<Image>(),
                ean: null,
                quantity: quantity,
                location: null,
                additionalDescription: null,
                reserved: reserved
            );
        }
    }
}
