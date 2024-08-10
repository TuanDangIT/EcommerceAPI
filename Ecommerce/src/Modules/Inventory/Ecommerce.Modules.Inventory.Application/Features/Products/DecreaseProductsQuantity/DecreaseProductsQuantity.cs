using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DecreaseProductsQuantity
{
    public sealed record class DecreaseProductsQuantity() : Shared.Abstractions.MediatR.ICommand;
}
