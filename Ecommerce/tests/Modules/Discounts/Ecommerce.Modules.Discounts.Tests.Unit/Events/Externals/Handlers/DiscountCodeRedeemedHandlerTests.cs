using Ecommerce.Modules.Discounts.Core.DAL;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Events.Externals;
using Ecommerce.Modules.Discounts.Core.Events.Externals.Handlers;
using Ecommerce.Shared.Abstractions.Events;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Discounts.Tests.Unit.Events.Externals.Handlers
{
    public class DiscountCodeRedeemedHandlerTests
    {
        private readonly IDiscountDbContext _dbContext;
        private readonly IEventHandler<DiscountCodeRedeemed> _handler;
        private readonly IQueryable<Discount> _querable;
        private readonly DateTime _correctDate;
        private readonly DateTime _now;
        public DiscountCodeRedeemedHandlerTests()
        {
            _dbContext = Substitute.For<IDiscountDbContext>();
            
            _handler = new DiscountCodeRedeemedHandler(_dbContext);
            _now = new DateTime(2022, 02, 25);
            _correctDate = new DateTime(2022, 02, 26);
        }
        [Fact]
        public async Task Handle_WithCorrectCodeInEvent_ShouldRedeem()
        {
            var @event = new DiscountCodeRedeemed("code");
            //var discount = new Core.Entities.Discount("code", "promotionCodeId", _correctDate, _now);
            var discount = Substitute.For<Core.Entities.Discount>("code", "promotionCodeId", _correctDate, _now);
            _dbContext.Discounts.Returns()
            _dbContext.Discounts
                .SingleOrDefault(d => d.Code == @event.Code)
                .Returns(discount);
            await _handler.HandleAsync(@event);
            discount.Received(1).Redeem();
            await _dbContext.Received(1).SaveChangesAsync();
        }
        [Fact]
        public void Handle_WithIncorrectCodeInEvent_ShouldFail()
        {

        }
    }
}
