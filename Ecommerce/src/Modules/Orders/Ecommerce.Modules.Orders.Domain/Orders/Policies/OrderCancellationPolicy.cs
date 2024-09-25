using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Policies
{
    internal class OrderCancellationPolicy : IOrderCancellationPolicy
    {
        private readonly TimeProvider _timeProvider;
        private const int _maxMinutesToBeAbleToCancel = 30;
        public OrderCancellationPolicy(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public Task<bool> CanCancel(Order order)
        {
            bool canDelete;
            var timeSpanAfterPlacingOrder = _timeProvider.GetUtcNow().UtcDateTime - order.OrderPlacedAt;
            if(timeSpanAfterPlacingOrder.TotalMinutes > _maxMinutesToBeAbleToCancel)
            {
                canDelete = false;
            }
            else
            {
                canDelete = true;
            }
            return Task.FromResult(canDelete);
        }
    }
}
