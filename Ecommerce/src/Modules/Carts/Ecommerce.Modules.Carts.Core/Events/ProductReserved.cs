using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events
{
    public sealed record class ProductReserved(Guid ProductId, int Quantity) : IEvent;
}
