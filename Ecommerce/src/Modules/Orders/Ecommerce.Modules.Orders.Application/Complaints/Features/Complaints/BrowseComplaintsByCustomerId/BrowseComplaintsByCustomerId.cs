using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.BrowseComplaintsByCustomerId
{
    public sealed class BrowseComplaintsByCustomerId : SieveModel, IQuery<PagedResult<ComplaintBrowseDto>>
    {
        [SwaggerIgnore]
        public Guid CustomerId { get; set; }
    }
}
