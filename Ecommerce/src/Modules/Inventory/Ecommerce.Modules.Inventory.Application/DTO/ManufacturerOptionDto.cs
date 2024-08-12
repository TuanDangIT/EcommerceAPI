using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.DTO
{
    public class ManufacturerOptionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
