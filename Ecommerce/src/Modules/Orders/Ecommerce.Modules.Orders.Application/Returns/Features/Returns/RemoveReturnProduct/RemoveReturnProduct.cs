using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.RemoveReturnProduct
{
    public sealed record class RemoveReturnProduct(Guid ReturnId, int ProductId) : ICommand;
}
