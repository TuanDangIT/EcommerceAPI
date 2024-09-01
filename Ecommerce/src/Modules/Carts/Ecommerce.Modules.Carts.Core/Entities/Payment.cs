using Ecommerce.Modules.Carts.Core.Entities.Enums;
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
        public PaymentMethod PaymentMethod { get; set; }
        public List<CheckoutCart> CheckoutCarts { get; set; } = new();
        public Payment(Guid id, PaymentMethod paymentMethod)
        {
            Id = id;
            PaymentMethod = paymentMethod;
        }
        public Payment()
        {
            
        }
    }
}
