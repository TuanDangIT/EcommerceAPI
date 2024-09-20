using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn
{
    internal sealed class HandleReturnHandler : ICommandHandler<HandleReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly TimeProvider _timeProvider;

        public HandleReturnHandler(IReturnRepository returnRepository, TimeProvider timeProvider)
        {
            _returnRepository = returnRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(HandleReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetReturnAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            @return.HandleReturn(_timeProvider.GetUtcNow().UtcDateTime);
            await _returnRepository.UpdateAsync();
        }
    }
}
