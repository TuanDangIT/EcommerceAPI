using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Sieve
{
    internal class DiscountsModuleSieveProcessor : SieveProcessor
    {
        public DiscountsModuleSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }
        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
