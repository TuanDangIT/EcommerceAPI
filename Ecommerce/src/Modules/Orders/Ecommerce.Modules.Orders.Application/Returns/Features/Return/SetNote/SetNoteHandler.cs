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

        public SetNoteHandler(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }
        public async Task Handle(SetNote request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            @return.SetNote(request.Note);
            await _returnRepository.UpdateAsync();
        }
    }
}
