﻿using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.CreateParameter
{
    internal sealed class CreateParameterHandler : ICommandHandler<CreateParameter>
    {
        private readonly IParameterRepository _parameterRepository;

        public CreateParameterHandler(IParameterRepository parameterRepository)
        {
            _parameterRepository = parameterRepository;
        }
        public async Task Handle(CreateParameter request, CancellationToken cancellationToken)
        {
            await _parameterRepository.AddAsync(new Parameter(Guid.NewGuid(), request.Name));
        }
    }
}
