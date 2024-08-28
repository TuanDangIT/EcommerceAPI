using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Events.External.Handlers
{
    internal class ProductUpdatedHandler : IEventHandler<ProductUpdated>
    {
        public Task HandleAsync(ProductUpdated @event)
        {
            throw new NotImplementedException();
        }
    }
}
