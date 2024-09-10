using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.CancelOrder
{
    internal class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
        public Task Handle(CancelOrder request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
