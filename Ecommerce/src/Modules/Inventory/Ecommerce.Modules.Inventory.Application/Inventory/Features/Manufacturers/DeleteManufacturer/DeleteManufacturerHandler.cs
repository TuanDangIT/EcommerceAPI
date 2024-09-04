using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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

        public DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task Handle(DeleteManufacturer request, CancellationToken cancellationToken)
            => await _manufacturerRepository.DeleteAsync(request.ManufacturerID);
        
    }
}
