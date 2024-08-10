﻿using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers
{
    internal sealed class DeleteSelectedManufacturersHandler : ICommandHandler<DeleteSelectedManufacturers>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public DeleteSelectedManufacturersHandler(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task Handle(DeleteSelectedManufacturers request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _manufacturerRepository.DeleteManyAsync(request.ManufacturerIds);
            if(rowsChanged != request.ManufacturerIds.Count())
            {
                throw new ManufacturerNotAllDeletedException();
            }
        }
    }
}
