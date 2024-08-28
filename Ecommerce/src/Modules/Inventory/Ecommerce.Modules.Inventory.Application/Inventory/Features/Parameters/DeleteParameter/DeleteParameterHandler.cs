using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.DeleteParameter
{
    internal sealed class DeleteParameterHandler : ICommandHandler<DeleteParameter>
    {
        private readonly IParameterRepository _parameterRepository;

        public DeleteParameterHandler(IParameterRepository parameterRepository)
        {
            _parameterRepository = parameterRepository;
        }
        public async Task Handle(DeleteParameter request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _parameterRepository.DeleteAsync(request.ParameterId);
            if (rowsChanged != 1)
            {
                throw new ParameterNotDeletedException(request.ParameterId);
            }
        }
    }
}
