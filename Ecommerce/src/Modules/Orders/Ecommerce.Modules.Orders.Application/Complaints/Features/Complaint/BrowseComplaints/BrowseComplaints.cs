using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints
{
    public sealed record class BrowseComplaints : IQuery<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>;
}
