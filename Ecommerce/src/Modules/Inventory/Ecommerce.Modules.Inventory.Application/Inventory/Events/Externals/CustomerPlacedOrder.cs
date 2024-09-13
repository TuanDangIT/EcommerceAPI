using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals
{
    public sealed record class CustomerPlacedOrder : IEvent
    {
        public IEnumerable<object> Products { get; set; } = Enumerable.Empty<object>();
    }
}
