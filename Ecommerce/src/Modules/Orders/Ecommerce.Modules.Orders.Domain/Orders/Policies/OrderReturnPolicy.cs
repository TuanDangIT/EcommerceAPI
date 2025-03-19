using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
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
        private readonly IContextService _contextService;
        private readonly int _maxDaysToBeAbleToReturn = 14;
        private readonly string[] _rolesThatCanForceReturnOrder = ["Employee", "Manager", "Admin"];
        public OrderReturnPolicy(TimeProvider timeProvider, IContextService contextService)
        {
            _timeProvider = timeProvider;
            _contextService = contextService;
        }
        public Task<bool> CanReturn(Order order)
        {
            bool canDelete;
            var timeSpanAfterPlacingOrder = _timeProvider.GetUtcNow().UtcDateTime - order.CreatedAt;
            if (timeSpanAfterPlacingOrder.TotalDays > _maxDaysToBeAbleToReturn && !_rolesThatCanForceReturnOrder.Contains(_contextService.Identity?.Role))
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
