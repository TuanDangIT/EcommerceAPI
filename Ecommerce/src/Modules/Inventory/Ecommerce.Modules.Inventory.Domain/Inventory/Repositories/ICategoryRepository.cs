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
        Task<int> AddAsync(Category category);
        Task<int> UpdateAsync(Category category);
        Task<int> DeleteAsync(Guid categoryId);
        Task<int> DeleteManyAsync(Guid[] categoryIds);
        //Task<IEnumerable<Parameter>> GetAllAsync();
        Task<Category?> GetAsync(Guid categoryId);
    }
}
