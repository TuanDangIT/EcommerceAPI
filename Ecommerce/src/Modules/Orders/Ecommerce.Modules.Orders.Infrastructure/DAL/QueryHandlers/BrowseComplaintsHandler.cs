using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseComplaintsHandler : IQueryHandler<BrowseComplaints, CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>
    {
        public Task<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>> Handle(BrowseComplaints request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
