using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer
{
    internal sealed class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        public Task Handle(DeleteManufacturer request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
