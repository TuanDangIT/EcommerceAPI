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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.ChangeParameterName
{
    internal sealed class ChangeParameterNameHandler : ICommandHandler<ChangeParameterName>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ILogger<ChangeParameterNameHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeParameterNameHandler(IParameterRepository parameterRepository, ILogger<ChangeParameterNameHandler> logger,
            IContextService contextService)
        {
            _parameterRepository = parameterRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeParameterName request, CancellationToken cancellationToken)
        {
            var parameter = await _parameterRepository.GetAsync(request.ParameterId);
            if (parameter is null)
            {
                throw new ParameterNotFoundException(request.ParameterId);
            }
            parameter.ChangeName(request.Name);
            await _parameterRepository.UpdateAsync();
            _logger.LogInformation("Parameter's: {parameter} name was changed to {newName} by {username}:{userId}.", parameter, request.Name,
                _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
