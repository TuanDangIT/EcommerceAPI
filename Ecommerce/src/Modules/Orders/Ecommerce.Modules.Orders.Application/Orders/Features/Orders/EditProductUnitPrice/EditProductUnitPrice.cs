using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.EditProductUnitPrice
{
    public sealed record class EditProductUnitPrice(Guid OrderId, int ProductId, decimal UnitPrice) : ICommand;
}
