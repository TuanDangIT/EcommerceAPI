using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Entities.ValueObjects
{
    public sealed record Product(string SKU, string Name, decimal Price, int Quantity);
}
