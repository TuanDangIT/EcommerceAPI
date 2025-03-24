using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.BrowseOrdersByCustomerId
{
    public sealed class BrowseOrdersByCustomerId : SieveModel, IQuery<PagedResult<OrderCustomerBrowseDto>>
    {
        [SwaggerIgnore]
        public Guid CustomerId { get; set; }
    }
}
