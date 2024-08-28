using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.DecreaseProductsQuantity
{
    public sealed record class DecreaseProductsQuantity(Guid ProductId, int Quantity) : Shared.Abstractions.MediatR.ICommand;
}
