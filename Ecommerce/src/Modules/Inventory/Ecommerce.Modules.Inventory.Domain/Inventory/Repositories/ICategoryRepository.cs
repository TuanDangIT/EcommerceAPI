using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken = default);
        Task AddManyAsync(IEnumerable<Category> categories, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(Guid[] categoryIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Category?> GetAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
