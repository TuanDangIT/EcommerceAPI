using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Abstractions.Events;
using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External
{
    public sealed record class ProductsListed(IEnumerable<Product> Products) : IEvent;
}
