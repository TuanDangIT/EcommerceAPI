using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.ChangeParameterName
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
            parameter.ChangeName(request.Name, _timeProvider.GetUtcNow().UtcDateTime);
            await _parameterRepository.UpdateAsync();
        }
    }
}
