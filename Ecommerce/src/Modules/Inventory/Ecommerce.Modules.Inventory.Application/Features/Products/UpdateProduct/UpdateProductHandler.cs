using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.UpdateProduct
{
    internal sealed class UpdateProductHandler : ICommandHandler<UpdateProduct>
    {
        public Task Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
