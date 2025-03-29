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
        Task<Guid> AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default);
        Task AddManyAsync(IEnumerable<Manufacturer> manufacturers, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid manufacturerId, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] manufacturerIds);
        Task<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Manufacturer?> GetAsync(Guid manufacturerId, CancellationToken cancellationToken = default);
    }
}
