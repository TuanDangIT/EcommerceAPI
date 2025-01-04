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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteSelectedManufacturers
{
    internal sealed class DeleteSelectedManufacturersHandler : ICommandHandler<DeleteSelectedManufacturers>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<DeleteSelectedManufacturers> _logger;
        private readonly IContextService _contextService;

        public DeleteSelectedManufacturersHandler(IManufacturerRepository manufacturerRepository, ILogger<DeleteSelectedManufacturers> logger,
            IContextService contextService)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteSelectedManufacturers request, CancellationToken cancellationToken)
        {
            await _manufacturerRepository.DeleteManyAsync(cancellationToken, request.ManufacturerIds);
            _logger.LogInformation("Selected manufacturers: {@manufacturerIds} were deleted by {@user}.",
                request.ManufacturerIds, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        
    }
}
