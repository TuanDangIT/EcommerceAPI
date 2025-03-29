using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.CreateParameter
{
    internal sealed class CreateParameterHandler : ICommandHandler<CreateParameter, Guid>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ILogger<CreateParameterHandler> _logger;
        private readonly IContextService _contextService;

        public CreateParameterHandler(IParameterRepository parameterRepository, ILogger<CreateParameterHandler> logger,
            IContextService contextService)
        {
            _parameterRepository = parameterRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<Guid> Handle(CreateParameter request, CancellationToken cancellationToken)
        {
            var parameter = new Parameter(request.Name);
            var parameterId = await _parameterRepository.AddAsync(parameter, cancellationToken);
            _logger.LogInformation("Parameter: {@parameter} was created by {@user}.",
                parameter, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return parameterId;

        }
    }
}
