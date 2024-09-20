using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.SetNote
{
    internal sealed class SetNoteHandler : ICommandHandler<SetNote>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly TimeProvider _timeProvider;

        public SetNoteHandler(IReturnRepository returnRepository, TimeProvider timeProvider)
        {
            _returnRepository = returnRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(SetNote request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetReturnAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            @return.SetNote(request.Note, _timeProvider.GetUtcNow().UtcDateTime);
            await _returnRepository.UpdateAsync();
        }
    }
}
