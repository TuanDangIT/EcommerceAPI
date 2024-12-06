using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage
{
    internal class BlobStorageOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
