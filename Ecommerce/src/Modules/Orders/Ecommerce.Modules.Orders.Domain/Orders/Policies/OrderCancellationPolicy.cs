using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Contexts;
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
        private readonly IContextService _contextService;
        private readonly int _maxMinutesToBeAbleToCancel = 30;
        private readonly string[] _rolesThatCanForceCancelOrder = ["Employee", "Manager", "Admin"];

        public OrderCancellationPolicy(TimeProvider timeProvider, IContextService contextService)
        {
            _timeProvider = timeProvider;
            _contextService = contextService;
        }
        public Task<bool> CanCancel(Order order)
        {
            bool canDelete;
            var timeSpanAfterPlacingOrder = _timeProvider.GetUtcNow().UtcDateTime - order.CreatedAt;
            if(timeSpanAfterPlacingOrder.TotalMinutes > _maxMinutesToBeAbleToCancel && !_rolesThatCanForceCancelOrder.Contains(_contextService.Identity?.Role))
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
