using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Postgres
{
    internal class PostgresOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
