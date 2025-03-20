using Ecommerce.Modules.Inventory.Application.DAL;
using Ecommerce.Shared.Abstractions.MediatR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Shared.Behaviors
{
    internal class InventoryUnitOfWorkBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public InventoryUnitOfWorkBehavior(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ICommand)
            {
                return await next();
            }
            using var transaction = _inventoryUnitOfWork.BeginTransaction();
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
