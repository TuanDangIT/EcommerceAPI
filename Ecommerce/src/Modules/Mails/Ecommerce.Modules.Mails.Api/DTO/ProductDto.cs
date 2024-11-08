using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    public sealed record ProductDto(string SKU, string Name, decimal Price, int Quantity);
}
