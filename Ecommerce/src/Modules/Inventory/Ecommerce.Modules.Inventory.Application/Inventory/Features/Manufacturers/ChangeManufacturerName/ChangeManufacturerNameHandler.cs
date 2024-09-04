using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public ChangeManufacturerNameHandler(IManufacturerRepository manufacturerRepository, TimeProvider timeProvider)
        {
            _manufacturerRepository = manufacturerRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ChangeManufacturerName request, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetAsync(request.ManufaturerId);
            if (manufacturer is null)
            {
                throw new ManufacturerNotFoundException(request.ManufaturerId);
            }
            manufacturer.ChangeName(request.Name, _timeProvider.GetUtcNow().UtcDateTime);
            await _manufacturerRepository.UpdateAsync();
        }
    }
}
