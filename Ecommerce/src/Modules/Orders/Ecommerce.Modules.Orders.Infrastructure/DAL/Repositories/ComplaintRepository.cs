using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
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
        public async Task CreateComplaintAsync(Complaint complaint)
        {
            await _dbContext.AddAsync(complaint);
            await _dbContext.SaveChangesAsync();
        }

        public Task<Complaint?> GetComplaintAsync(Guid complaintId)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
