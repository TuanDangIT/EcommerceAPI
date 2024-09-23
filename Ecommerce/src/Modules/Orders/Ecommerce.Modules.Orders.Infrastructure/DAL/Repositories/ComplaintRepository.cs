using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
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
        public async Task CreateAsync(Complaint complaint)
        {
            await _dbContext.AddAsync(complaint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Complaint?> GetAsync(Guid complaintId)
            => await _dbContext.Complaints
                .Include(c => c.Order)
                .SingleOrDefaultAsync(c => c.Id == complaintId);    
        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
