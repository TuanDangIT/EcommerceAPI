﻿using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Discounts.Tests.Unit.Entities.Offer
{
    public class OfferEvaluateTests
    {
        private readonly DateTime _correctDate;
        private readonly DateTime _now;
        private readonly DateTime _incorrectDate;

        public OfferEvaluateTests()
        {
            _now = new DateTime(2022, 02, 25);
            _incorrectDate = new DateTime(2022, 01, 01);
            _correctDate = new DateTime(2022, 02, 26);
        }

        [Theory]
        [MemberData(nameof(GetOffersForAcceptOffer))]
        public void AcceptOffer_WtihInitializedOrRejectedStatusAndCorrectDate_ShouldChangeStatusToAccepted(Core.Entities.Offer offer)
        {
            // Act
            offer.Accept(_correctDate, _now);

            // Assert
            offer.Status.Should().Be(Core.Entities.Enums.OfferStatus.Accepted);
        }

        [Fact]
        public void AcceptOffer_WtihAcceptedStatusAndCorrectDate_ShouldThrowOfferCannotAcceptException()
        {
            // Arrange
            var offer = GetOffer(true);

            // Act
            var exception = Record.Exception(() => offer.Accept(_correctDate, _now));

            // Assert
            exception.Should().BeOfType<OfferCannotAcceptException>();
        }

        [Fact]
        public void AcceptOffer_WithInitializedOrRejectedStatusAndIncorrectDate_ShouldThrowOfferInvalidExpiresAtException()
        {
            // Arrange
            var offer = GetOffer(false);

            // Act
            var exception = Record.Exception(() => offer.Accept(_incorrectDate, _now));

            // Assert
            exception.Should().BeOfType<OfferInvalidExpiresAtException>();
        }

        [Theory]
        [MemberData(nameof(GetOffersForRejectOffer))]
        public void RejectOffer_ShouldChangeStatusToRejected(Core.Entities.Offer offer)
        {
            // Act
            offer.Reject();

            // Assert
            offer.Status.Should().Be(Core.Entities.Enums.OfferStatus.Rejected);
        }

        public static IEnumerable<object[]> GetOffersForAcceptOffer()
        {
            yield return new object[]
            {
                new Core.Entities.Offer("sku", "productName", 5, 10, "reason", Guid.NewGuid())
            };
            var offerToBeRejected = new Core.Entities.Offer("sku", "productName", 5, 10, "reason", Guid.NewGuid());
            offerToBeRejected.Reject();
            yield return new object[]
            {
                offerToBeRejected
            };
        }

        public static IEnumerable<object[]> GetOffersForRejectOffer()
        {
            yield return new object[]
            {
                new Core.Entities.Offer("sku", "productName", 5, 10, "reason", Guid.NewGuid())
            };
            var offerToBeAccepted = new Core.Entities.Offer("sku", "productName", 5, 10, "reason", Guid.NewGuid());
            offerToBeAccepted.Accept(new DateTime(2022, 02, 26), new DateTime(2022, 02, 25));
            yield return new object[]
            {
                offerToBeAccepted
            };
        }

        private Core.Entities.Offer GetOffer(bool isAccepted)
        {
            var offer = new Core.Entities.Offer("sku", "productName", 5, 10, "reason", Guid.NewGuid());
            if (isAccepted)
            {
                offer.Accept(_correctDate, _now);
            }
            return offer;
        }
    }
}