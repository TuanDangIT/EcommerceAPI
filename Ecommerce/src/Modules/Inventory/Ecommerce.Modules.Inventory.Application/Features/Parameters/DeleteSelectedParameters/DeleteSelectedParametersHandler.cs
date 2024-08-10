using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteSelectedParameters
{
    internal sealed class DeleteSelectedParametersHandler : ICommandHandler<DeleteSelectedParameters>
    {
        private readonly IParameterRepository _parameterRepository;

        public DeleteSelectedParametersHandler(IParameterRepository parameterRepository)
        {
            _parameterRepository = parameterRepository;
        }
        public async Task Handle(DeleteSelectedParameters request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _parameterRepository.DeleteManyAsync(request.ParameterIds);
            if (rowsChanged != request.ParameterIds.Count())
            {
                throw new ParameterNotAllDeletedException();
            }
        }
    }
}
