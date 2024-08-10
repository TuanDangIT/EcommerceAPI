using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Repositories
{
    public interface IManufacturerRepository 
    {
        Task<int> AddAsync(Manufacturer manufacturer);
        Task<int> UpdateAsync(Manufacturer manufacturer);
        Task<int> DeleteAsync(Guid manufacturerId);
        Task<int> DeleteManyAsync(params Guid[] manufacturerIds);
        //Task<IEnumerable<Manufacturer>> GetAllAsync();
        //Task<Manufacturer?> GetAsync(Guid manufacturerId);
    }
}
