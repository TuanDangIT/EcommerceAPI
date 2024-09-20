using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint
{
    public sealed record class SubmitComplaint(string Title, string Description, Guid OrderId) : ICommand;
}
