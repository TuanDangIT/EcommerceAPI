using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteManufacturer
{
    internal sealed class DeleteManufacturerHandler : ICommandHandler<DeleteManufacturer>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task Handle(DeleteManufacturer request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _manufacturerRepository.DeleteAsync(request.ManufacturerID);
            if(rowsChanged is not 1)
            {
                throw new ManufacturerNotDeletedException(request.ManufacturerID);
            }
        }
    }
}
