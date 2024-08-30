using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class Payment
    {
        public Guid Id { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public Payment(Guid id, string paymentMethod)
        {
            Id = id;
            PaymentMethod = paymentMethod;
        }
        public Payment()
        {
            
        }
    }
}
