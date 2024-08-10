using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Contexts
{
    public interface IContextService
    {
        string RequestId { get; }
        string TraceId { get; }
        IIdentityContext? Identity { get; }
    }
}
