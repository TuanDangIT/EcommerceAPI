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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteManufacturer
{
    internal sealed class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<DeleteManufacturerHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository, ILogger<DeleteManufacturerHandler> logger,
            IContextService contextService)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteManufacturer request, CancellationToken cancellationToken)
        {
            await _manufacturerRepository.DeleteAsync(request.ManufacturerId, cancellationToken);
            _logger.LogInformation("Manufacturer: {manufacturerId} was deleted by {@user}.", request.ManufacturerId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        
    }
}
