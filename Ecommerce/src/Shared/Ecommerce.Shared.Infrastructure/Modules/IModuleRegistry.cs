using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Modules
{
    public interface IModuleRegistry
    {
        IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
        void AddBroadcastAction(Type requestType, Func<object, Task> action);
    }
}
