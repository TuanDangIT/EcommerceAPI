using Ecommerce.Modules.Carts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class PaymentDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public bool IsActived { get; set; } 
    }
}
