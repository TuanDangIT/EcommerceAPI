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

        public async Task CreateAsync(Invoice invoice, CancellationToken cancellationToken = default)
        {
            await _dbContext.Invoices.AddAsync(invoice, cancellationToken); 
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int invoiceId, CancellationToken cancellationToken = default)
            => await _dbContext.Invoices.Where(i => i.Id == invoiceId)
                .ExecuteDeleteAsync(cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
