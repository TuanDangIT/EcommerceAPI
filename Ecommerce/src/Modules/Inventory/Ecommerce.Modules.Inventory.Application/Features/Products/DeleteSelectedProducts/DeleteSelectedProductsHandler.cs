using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductsHandler : ICommandHandler<DeleteSelectedProducts>
    {
        public Task Handle(DeleteSelectedProducts request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
