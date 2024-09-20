using Ecommerce.Modules.Orders.Domain.Returns.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Repositories
{
    public interface IReturnRepository
    {
        Task CreateReturnAsync(Return @return);
        Task<Return?> GetReturnByOrderIdAsync(Guid orderId);
        Task<Return?> GetReturnAsync(Guid returnId);
        Task UpdateAsync();
    }
}
