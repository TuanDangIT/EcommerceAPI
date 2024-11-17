using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories
{
    internal class InvoiceRepository : IInvoiceRepository
    {
        private readonly OrdersDbContext _dbContext;

        public InvoiceRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Invoice invoice)
        {
            await _dbContext.Invoices.AddAsync(invoice); 
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int invoiceId)
            => await _dbContext.Invoices.Where(i => i.Id == invoiceId)
                .ExecuteDeleteAsync();

        public async Task<Invoice?> GetAsync(int invoiceId)
            => await _dbContext.Invoices
                .Include(i => i.Order)
                .SingleOrDefaultAsync(i => i.Id == invoiceId);

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
