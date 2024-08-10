using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.CreateManufacturer
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
            var rowsChanged = await _manufacturerRepository.AddAsync(new Domain.Entities.Manufacturer()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
            });
            if(rowsChanged is not 1)
            {
                throw new ManufacturerNotCreatedException();
            }
        }
    }
}
