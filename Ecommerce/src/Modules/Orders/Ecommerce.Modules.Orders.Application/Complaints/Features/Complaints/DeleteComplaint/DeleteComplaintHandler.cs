using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.DeleteComplaint
{
    internal class DeleteComplaintHandler : ICommandHandler<DeleteComplaint>
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly ILogger<DeleteComplaintHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteComplaintHandler(IComplaintRepository complaintRepository, ILogger<DeleteComplaintHandler> logger, IContextService contextService)
        {
            _complaintRepository = complaintRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteComplaint request, CancellationToken cancellationToken)
        {
            await _complaintRepository.DeleteAsync(request.ComplaintId, cancellationToken);
            _logger.LogInformation("Complaint: {complaintId} was deleted by {@user}.", request.ComplaintId, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
