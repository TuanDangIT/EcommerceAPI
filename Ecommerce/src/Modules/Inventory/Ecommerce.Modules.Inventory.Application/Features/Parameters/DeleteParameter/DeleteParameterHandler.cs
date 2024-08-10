using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteParameter
{
    internal sealed class DeleteParameterHandler : ICommandHandler<DeleteParameter>
    {
        public Task Handle(DeleteParameter request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
