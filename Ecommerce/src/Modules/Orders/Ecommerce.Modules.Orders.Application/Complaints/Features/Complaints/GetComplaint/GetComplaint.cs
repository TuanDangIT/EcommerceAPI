using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.GetComplaint
{
    public sealed record class GetComplaint(Guid ComplaintId) : IQuery<ComplaintDetailsDto?>;
}
