using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External.Handlers
{
    internal class ProductUnlistedHandler : IEventHandler<ProductUnlisted>
    {
        public Task HandleAsync(ProductUnlisted @event)
        {
            throw new NotImplementedException();
        }
    }
}
