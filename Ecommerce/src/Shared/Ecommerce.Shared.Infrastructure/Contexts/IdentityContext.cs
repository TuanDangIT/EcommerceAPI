using Ecommerce.Shared.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    internal class IdentityContext : IIdentityContext
    {
        public bool IsAuthenticated { get; }
        public Guid Id { get; }
        public string Role { get; }

        public IdentityContext(ClaimsPrincipal principal)
        {
            if(principal.Identity?.IsAuthenticated is false)
            {
                IsAuthenticated = false;
            }
            else
            {
                IsAuthenticated = true;
            }
            Id = IsAuthenticated ? Guid.Parse(principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value) : Guid.Empty;
            Role = IsAuthenticated ? principal.Claims.Single(x => x.Type == ClaimTypes.Role).Value : string.Empty;
        }
    }
}
