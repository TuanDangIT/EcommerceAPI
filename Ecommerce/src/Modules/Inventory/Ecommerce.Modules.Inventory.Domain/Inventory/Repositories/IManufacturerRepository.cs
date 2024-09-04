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
        Task AddAsync(Manufacturer manufacturer);
        Task UpdateAsync();
        Task DeleteAsync(Guid manufacturerId);
        Task DeleteManyAsync(Guid[] manufacturerIds);
        //Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetAsync(Guid manufacturerId);
    }
}
