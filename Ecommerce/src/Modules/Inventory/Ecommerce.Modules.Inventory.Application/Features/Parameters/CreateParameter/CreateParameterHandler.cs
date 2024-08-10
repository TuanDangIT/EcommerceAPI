using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.CreateParameter
{
    internal sealed class CreateParameterHandler : ICommandHandler<CreateParameter>
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly TimeProvider _timeProvider;

        public CreateParameterHandler(IParameterRepository parameterRepository, TimeProvider timeProvider)
        {
            _parameterRepository = parameterRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(CreateParameter request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _parameterRepository.AddAsync(new Domain.Entities.Parameter()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
            });
            if(rowsChanged is 0)
            {
                throw new ParameterNotCreatedException();
            }
        }
    }
}
