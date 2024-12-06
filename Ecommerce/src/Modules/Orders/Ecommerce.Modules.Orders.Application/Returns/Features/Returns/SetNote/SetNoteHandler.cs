using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SetNoteHandler> _logger;
        private readonly IContextService _contextService;

        public SetNoteHandler(IReturnRepository returnRepository, ILogger<SetNoteHandler> logger, IContextService contextService)
        {
            _returnRepository = returnRepository;
            _logger = logger;
            _contextService = contextService;
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
            _logger.LogInformation("Note: {note} was set for return: {return} by {username}:{userId}.", request.Note, @return
                , _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
