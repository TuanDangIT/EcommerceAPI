using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Discounts.Tests.Unit.Entities.Offer
{
    public class OfferSetExpiresTimeTests
    {
        private readonly DateTime _correctDate;
        private readonly DateTime _now;
        private readonly DateTime _incorrectDate;

        public OfferSetExpiresTimeTests()
        {
            _now = new DateTime(2022, 02, 25);
            _incorrectDate = new DateTime(2022, 01, 01);
            _correctDate = new DateTime(2022, 02, 26);
        }

        [Fact]
        public void SetExpiresTime_WithCorrectDate_ShouldSucceed()
        {
            // Arrange
            var offer = GetOffer();

            // Act
            offer.SetExpiresTime(_correctDate, _now);

            //Assert
            offer.ExpiresAt.Should().NotBeNull();
            offer.ExpiresAt.Should().Be(_correctDate);
        }

        [Fact]
        public void SetExpiresTime_WithIncorrectDate_ShouldThrowOfferInvalidExpiresAtException()
        {
            // Arrange
            var offer = GetOffer();

            // Act
            Action action = () => offer.SetExpiresTime(_incorrectDate, _now);

            // Assert
            action.Invoking(a => a.Invoke()).Should().Throw<OfferInvalidExpiresAtException>();
        }

        private Core.Entities.Offer GetOffer()
            => new("sku", "productName", 5, 10, "reason", Guid.NewGuid());
    }
}