using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteParameter
{
    internal sealed class DeleteParameterHandler : ICommandHandler<DeleteParameter>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ILogger<DeleteParameterHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteParameterHandler(IParameterRepository parameterRepository, ILogger<DeleteParameterHandler> logger,
            IContextService contextService)
        {
            _parameterRepository = parameterRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteParameter request, CancellationToken cancellationToken)
        {
            await _parameterRepository.DeleteAsync(request.ParameterId);
            _logger.LogInformation("Parameter: {parameterId} was deleted by {user}.", request.ParameterId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
