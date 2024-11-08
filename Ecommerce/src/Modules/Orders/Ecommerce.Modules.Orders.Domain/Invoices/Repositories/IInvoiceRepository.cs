using Ecommerce.Modules.Orders.Domain.Invoices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Invoices.Repositories
{
    public interface IInvoiceRepository
    {
        Task CreateAsync(Invoice invoice);
        Task<Invoice?> GetAsync(int invoiceId);
        Task DeleteAsync(int invoiceId);
        Task UpdateAsync();
    }
}
