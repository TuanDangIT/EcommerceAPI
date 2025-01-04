using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.AddProduct
{
    public sealed record class AddProduct(Guid OrderId, int ProductId, string SKU, string? Name, decimal? UnitPrice, int Quantity, string? ImagePathUrl) : ICommand;
}
