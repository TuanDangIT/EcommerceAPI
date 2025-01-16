using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductQuantity
{
    public sealed record class SetReturnProductQuantity(Guid ReturnId, int ProductId, int Quantity) : ICommand;
}
