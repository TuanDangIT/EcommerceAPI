using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteSelectedParameters
{
    internal sealed class DeleteSelectedParametersHandler : ICommandHandler<DeleteSelectedParameters>
    {
        public Task Handle(DeleteSelectedParameters request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
