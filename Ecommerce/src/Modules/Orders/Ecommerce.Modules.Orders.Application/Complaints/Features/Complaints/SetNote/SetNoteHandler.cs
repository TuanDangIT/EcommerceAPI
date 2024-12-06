using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.SetNote
{
    internal sealed class SetNoteHandler : ICommandHandler<SetNote>
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly ILogger<SetNoteHandler> _logger;
        private readonly IContextService _contextService;

        public SetNoteHandler(IComplaintRepository complaintRepository, ILogger<SetNoteHandler> logger, IContextService contextService)
        {
            _complaintRepository = complaintRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(SetNote request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            if(complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            complaint.SetNote(request.Note);
            await _complaintRepository.UpdateAsync();
            _logger.LogInformation("Note: {note} was set for complaint: {complaint} by {username}:{userId}.", request.Note, complaint, _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
