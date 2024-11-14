using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ChangeProductQuantity
{
    public sealed record class ChangeProductQuantity(Guid ProductId, int Quantity) : ICommand;
}
