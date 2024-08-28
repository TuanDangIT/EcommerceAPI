using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DeleteSelectedProducts
{
    public sealed record class DeleteSelectedProducts(Guid[] ProductIds) : Shared.Abstractions.MediatR.ICommand;
}
