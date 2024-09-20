using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Repositories
{
    public interface IComplaintRepository
    {
        Task CreateComplaintAsync(Complaint complaint);
        Task<Complaint?> GetComplaintAsync(Guid complaintId);
        Task UpdateAsync();
    }
}
