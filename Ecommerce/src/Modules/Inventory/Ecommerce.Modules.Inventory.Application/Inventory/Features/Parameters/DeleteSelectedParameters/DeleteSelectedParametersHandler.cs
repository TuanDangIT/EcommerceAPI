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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteSelectedParameters
{
    internal sealed class DeleteSelectedParametersHandler : ICommandHandler<DeleteSelectedParameters>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ILogger<DeleteSelectedParametersHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteSelectedParametersHandler(IParameterRepository parameterRepository, ILogger<DeleteSelectedParametersHandler> logger,
            IContextService contextService)
        {
            _parameterRepository = parameterRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteSelectedParameters request, CancellationToken cancellationToken)
        {
            await _parameterRepository.DeleteManyAsync(request.ParameterIds, cancellationToken);
            _logger.LogInformation("Selected parameters: {parameterIds} were deleted by {@user}.",
                request.ParameterIds, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
