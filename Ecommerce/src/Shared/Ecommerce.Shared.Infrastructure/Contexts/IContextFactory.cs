using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    internal interface IContextFactory
    {
        IContextService Create();
    }
}
