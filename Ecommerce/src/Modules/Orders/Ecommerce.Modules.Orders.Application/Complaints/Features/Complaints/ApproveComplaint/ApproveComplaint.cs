using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.ApproveComplaint
{
    public sealed record class ApproveComplaint(DecisionApproveOrEditDto Decision) : ICommand
    {
        [SwaggerIgnore]
        public Guid ComplaintId { get; init; }
    }
}
