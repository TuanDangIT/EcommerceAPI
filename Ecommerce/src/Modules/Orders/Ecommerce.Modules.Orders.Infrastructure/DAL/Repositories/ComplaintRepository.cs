using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories
{
    internal class ComplaintRepository : IComplaintRepository
    {
        private readonly OrdersDbContext _dbContext;

        public ComplaintRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Complaint complaint, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(complaint, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Complaint?> GetAsync(Guid complaintId, CancellationToken cancellationToken = default, params Func<IQueryable<Complaint>, IQueryable<Complaint>>[] includeActions)
        {
            var query = _dbContext.Complaints
                .AsQueryable();
            if (includeActions is not null)
            {
                foreach (var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var complaint = await query
                .FirstOrDefaultAsync(r => r.Id == complaintId, cancellationToken);
            return complaint;
        }
        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

        public async Task DeleteAsync(Guid complaintId, CancellationToken cancellationToken = default)
            => await _dbContext.Complaints
                .Where(c => c.Id == complaintId)
                .ExecuteDeleteAsync(cancellationToken);
    }
}
