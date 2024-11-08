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

        public CreateManufacturerHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task Handle(CreateManufacturer request, CancellationToken cancellationToken)
        {
            await _manufacturerRepository.AddAsync(new Manufacturer(Guid.NewGuid(), request.Name));
        }
    }
}
