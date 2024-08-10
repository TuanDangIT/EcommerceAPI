using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.CreateProduct
{
    internal sealed class CreateProductHandler : ICommandHandler<CreateProduct>
    {
        public Task Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
