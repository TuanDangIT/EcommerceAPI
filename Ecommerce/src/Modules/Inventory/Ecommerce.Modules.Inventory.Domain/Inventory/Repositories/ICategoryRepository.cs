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
        Task AddAsync(Category category);
        Task UpdateAsync();
        Task DeleteAsync(Guid categoryId);
        Task DeleteManyAsync(Guid[] categoryIds);
        //Task<IEnumerable<Parameter>> GetAllAsync();
        Task<Category?> GetAsync(Guid categoryId);
    }
}
