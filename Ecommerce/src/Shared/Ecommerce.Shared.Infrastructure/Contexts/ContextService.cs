using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    internal class ContextService : IContextService
    {
        public string RequestId => $"{Guid.NewGuid()}";

        public string TraceId { get; } = string.Empty;

        public IIdentityContext? Identity { get; }
        internal ContextService()
        {
        }
        public ContextService(HttpContext context) : this(context.TraceIdentifier, new IdentityContext(context.User))
        {
        }

        internal ContextService(string traceId, IIdentityContext identity)
        {
            TraceId = traceId;
            Identity = identity;
        }
    }
}
