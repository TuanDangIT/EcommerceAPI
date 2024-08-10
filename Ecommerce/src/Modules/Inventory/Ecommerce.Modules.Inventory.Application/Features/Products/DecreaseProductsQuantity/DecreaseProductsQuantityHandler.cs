using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DecreaseProductsQuantity
{
    internal class DecreaseProductsQuantityHandler : ICommandHandler<DecreaseProductsQuantity>
    {
        public Task Handle(DecreaseProductsQuantity request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
