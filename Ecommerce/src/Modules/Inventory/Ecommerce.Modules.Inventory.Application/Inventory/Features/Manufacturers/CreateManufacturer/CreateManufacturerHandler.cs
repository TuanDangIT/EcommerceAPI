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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.CreateManufacturer
{
    internal sealed class CreateManufacturerHandler : ICommandHandler<CreateManufacturer>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILogger<CreateManufacturerHandler> _logger;
        private readonly IContextService _contextService;

        public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository, ILogger<CreateManufacturerHandler> logger,
            IContextService contextService)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(CreateManufacturer request, CancellationToken cancellationToken)
        {
            var manufacturer = new Manufacturer(request.Name);
            await _manufacturerRepository.AddAsync(manufacturer, cancellationToken);
            _logger.LogInformation("Manufacturer: {@manufacturer} was created by {@user}.",
                manufacturer, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
