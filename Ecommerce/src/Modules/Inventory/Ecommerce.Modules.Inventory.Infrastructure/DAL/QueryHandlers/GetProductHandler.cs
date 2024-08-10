using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Products.GetProduct;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetProductHandler : IQueryHandler<GetProduct, ProductDto>
    {
        public Task<ProductDto> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
