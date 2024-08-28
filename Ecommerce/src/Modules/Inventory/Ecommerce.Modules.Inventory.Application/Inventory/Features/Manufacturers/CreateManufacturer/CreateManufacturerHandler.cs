using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository, TimeProvider timeProvider)
        {
            _manufacturerRepository = manufacturerRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(CreateManufacturer request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _manufacturerRepository.AddAsync(new Manufacturer(Guid.NewGuid(), request.Name, _timeProvider.GetUtcNow().UtcDateTime));
            if (rowsChanged != 1)
            {
                throw new ManufacturerNotCreatedException();
            }
        }
    }
}
