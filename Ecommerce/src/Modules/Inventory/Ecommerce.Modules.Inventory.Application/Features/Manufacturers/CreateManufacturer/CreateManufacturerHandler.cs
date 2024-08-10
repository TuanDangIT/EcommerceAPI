using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer
{
    internal sealed class CreateManufacturerHandler : ICommandHandler<CreateManufacturer>
    {
        public Task Handle(CreateManufacturer request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
