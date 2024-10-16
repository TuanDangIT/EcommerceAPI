using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Policies
{
    public interface IOrderInvoiceCreationPolicy
    {
        Task<bool> CanCreateInvoice(Order order);
    }
}
