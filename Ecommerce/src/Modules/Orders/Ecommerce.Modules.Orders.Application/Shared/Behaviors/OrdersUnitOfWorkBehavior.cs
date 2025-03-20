using Ecommerce.Modules.Orders.Application.Shared.UnitOfWork;
using Ecommerce.Shared.Abstractions.MediatR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shared.Behaviors
{
    internal class OrdersUnitOfWorkBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IOrdersUnitOfWork _ordersUnitOfWork;

        public OrdersUnitOfWorkBehavior(IOrdersUnitOfWork ordersUnitOfWork)
        {
            _ordersUnitOfWork = ordersUnitOfWork;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(request is not ICommand)
            {
                return await next();
            }
            using var transaction = _ordersUnitOfWork.BeginTransaction();
            try
            {
                var response = await next();
                transaction.Commit();

                return response;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
