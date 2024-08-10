using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    internal class ContextFactory 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IContextService Create()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            return httpContext is null ? new ContextService() : new ContextService(httpContext);
        }
    }
}
