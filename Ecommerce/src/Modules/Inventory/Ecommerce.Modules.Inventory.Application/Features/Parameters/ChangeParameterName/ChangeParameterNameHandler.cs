using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.ChangeParameterName
{
    internal sealed class ChangeParameterNameHandler : ICommandHandler<ChangeParameterName>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly TimeProvider _timeProvider;

        public ChangeParameterNameHandler(IParameterRepository parameterRepository, TimeProvider timeProvider)
        {
            _parameterRepository = parameterRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ChangeParameterName request, CancellationToken cancellationToken)
        {
            var parameter = await _parameterRepository.GetAsync(request.ParameterId);
            if (parameter is null)
            {
                throw new ParameterNotFoundException(request.ParameterId);
            }
            parameter.Name = request.Name;
            parameter.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            var rowsChanged = await _parameterRepository.UpdateAsync(parameter);
            if (rowsChanged is not 1)
            {
                throw new ParameterNotUpdatedException(request.ParameterId);
            }
        }
    }
}
