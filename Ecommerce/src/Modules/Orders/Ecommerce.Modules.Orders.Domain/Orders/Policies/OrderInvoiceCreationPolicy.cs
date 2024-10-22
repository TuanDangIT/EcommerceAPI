using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Policies
{
    internal class OrderInvoiceCreationPolicy : IOrderInvoiceCreationPolicy
    {
        public Task<bool> CanCreateInvoice(Order order)
        {
            if(order.Invoice is not null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
