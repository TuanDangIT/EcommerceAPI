using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Modules
{
    public sealed class ModuleBroadcastRegistration
    {
        public Type ReceiverType { get; }
        public Func<object, Task> Action { get; }
        public string Key => ReceiverType.Name;

        public ModuleBroadcastRegistration(Type receiverType, Func<object, Task> action)
        {
            ReceiverType = receiverType;
            Action = action;
        }
    }
}
