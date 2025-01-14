using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events
{
    internal record class ProductsUnreserved(Dictionary<Guid, int> Products) : IEvent;
}
