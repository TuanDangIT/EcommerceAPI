﻿using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.CreateParameter
{
    internal sealed class CreateParameterHandler : ICommandHandler<CreateParameter>
    {
        public Task Handle(CreateParameter request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
