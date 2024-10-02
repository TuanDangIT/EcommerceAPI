using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class AvailablePaymentDto
    {
        public Guid Id { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
