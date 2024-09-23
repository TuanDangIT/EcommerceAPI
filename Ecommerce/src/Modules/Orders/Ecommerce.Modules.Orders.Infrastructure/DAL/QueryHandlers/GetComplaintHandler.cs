using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.GetComplaint;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetComplaintHandler : IQueryHandler<GetComplaint, ComplaintDetailsDto?>
    {
        private readonly IComplaintRepository _complaintRepository;

        public GetComplaintHandler(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }
        public async Task<ComplaintDetailsDto?> Handle(GetComplaint request, CancellationToken cancellationToken)
        {
            var complaint = await _complaintRepository.GetAsync(request.ComplaintId);
            return complaint?.AsDetailsDto();
        }
    }
}
