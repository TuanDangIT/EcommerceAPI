using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Repositories
{
    public interface IInvoiceRepository
    {
        Task CreateAsync(Invoice invoice);
        Task<Invoice?> GetAsync(int invoiceId);
        Task DeleteAsync(int invoiceId);
        Task UpdateAsync();
    }
}
