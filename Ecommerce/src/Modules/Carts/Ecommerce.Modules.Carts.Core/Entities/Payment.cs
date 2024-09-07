using Ecommerce.Modules.Carts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public List<CheckoutCart> CheckoutCarts { get; private set; } = [];
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
