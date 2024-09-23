using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CheckoutStripeSessionDto
    {
        public string SessionId { get; set; } = string.Empty;
        public string PaymentIntendId { get; set; } = string.Empty;
        public string CheckoutUrl { get; set; } = string.Empty;
    }
}
