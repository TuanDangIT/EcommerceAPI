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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.ChangeManufacturerName
{
    internal sealed class ChangeManufacturerNameHandler : ICommandHandler<ChangeManufacturerName>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<ChangeManufacturerNameHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeManufacturerNameHandler(IManufacturerRepository manufacturerRepository, ILogger<ChangeManufacturerNameHandler> logger,
            IContextService contextService)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeManufacturerName request, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(request.ManufaturerId);
            if (manufacturer is null)
            {
                throw new ManufacturerNotFoundException(request.ManufaturerId);
            }
            manufacturer.ChangeName(request.Name);
            await _manufacturerRepository.UpdateAsync();
            _logger.LogInformation("Manufacturer: {manufacturer} was changed by {username}:{userId}.", manufacturer,
                _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
