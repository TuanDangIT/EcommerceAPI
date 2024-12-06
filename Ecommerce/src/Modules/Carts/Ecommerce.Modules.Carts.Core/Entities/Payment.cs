using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Payment : BaseEntity
    {
        public PaymentMethod PaymentMethod { get; private set; }
        public bool IsActive { get; private set; } = true;
        public List<CheckoutCart> CheckoutCarts { get; private set; } = [];
        public Payment(Guid id, PaymentMethod paymentMethod)
        {
            Id = id;
            PaymentMethod = paymentMethod;
        }
        private Payment()
        {
            
        }
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
