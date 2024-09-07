using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Entities
{
    public class AuctionParameter
    {
        public string Name { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;
        public AuctionParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
