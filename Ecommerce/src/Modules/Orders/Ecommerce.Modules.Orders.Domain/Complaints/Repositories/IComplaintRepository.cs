using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Repositories
{
    public interface IComplaintRepository
    {
        Task CreateAsync(Complaint complaint, CancellationToken cancellationToken = default);
        Task<Complaint?> GetAsync(Guid complaintId, CancellationToken cancellationToken = default, 
            params Func<IQueryable<Complaint>, IQueryable<Complaint>>[] includeActions);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid complaintId, CancellationToken cancellationToken = default);
    }
}
