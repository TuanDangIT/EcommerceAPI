using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.BrowseReturnsByCustomerId
{
    public sealed class BrowseReturnsByCustomerId : SieveModel, IQuery<PagedResult<ReturnBrowseDto>>
    {
        [SwaggerIgnore]
        public Guid CustomerId { get; set; }
    }
}
