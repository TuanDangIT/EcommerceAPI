using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IManufacturerRepository
    {
        Task<int> AddAsync(Manufacturer manufacturer);
        Task<int> UpdateAsync(Manufacturer manufacturer);
        Task<int> DeleteAsync(Guid manufacturerId);
        Task<int> DeleteManyAsync(Guid[] manufacturerIds);
        //Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetAsync(Guid manufacturerId);
    }
}
