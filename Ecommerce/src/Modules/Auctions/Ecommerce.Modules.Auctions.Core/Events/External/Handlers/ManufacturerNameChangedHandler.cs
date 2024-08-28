using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Events.External.Handlers
{
    internal class ManufacturerNameChangedHandler : IEventHandler<ManufacturerNameChanged>
    {
        public Task HandleAsync(ManufacturerNameChanged @event)
        {
            throw new NotImplementedException();
        }
    }
}
