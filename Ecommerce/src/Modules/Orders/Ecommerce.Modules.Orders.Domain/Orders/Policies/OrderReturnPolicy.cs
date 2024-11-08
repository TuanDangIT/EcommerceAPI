using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Policies
{
    internal class OrderReturnPolicy : IOrderReturnPolicy
    {
        private readonly TimeProvider _timeProvider;
        private const int _maxDaysToBeAbleToReturn = 14;
        public OrderReturnPolicy(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        public Task<bool> CanReturn(Order order)
        {
            bool canDelete;
            var timeSpanAfterPlacingOrder = _timeProvider.GetUtcNow().UtcDateTime - order.CreatedAt;
            if (timeSpanAfterPlacingOrder.TotalDays > _maxDaysToBeAbleToReturn)
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
