using Ecommerce.Modules.Orders.Application.Complaints.Exceptions;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Entity;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public SetNoteHandler(IComplaintRepository complaintRepository, TimeProvider timeProvider)
        {
            _complaintRepository = complaintRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(SetNote request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetComplaintAsync(request.ComplaintId);
            if(complaint is null)
            {
                throw new ComplaintNotFoundException(request.ComplaintId);
            }
            complaint.SetNote(request.Note, _timeProvider.GetUtcNow().UtcDateTime);
            await _complaintRepository.UpdateAsync();
        }
    }
}
