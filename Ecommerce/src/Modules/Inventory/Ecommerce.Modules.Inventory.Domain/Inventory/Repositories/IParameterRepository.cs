using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IParameterRepository
    {
        Task<Guid> AddAsync(Parameter parameter, CancellationToken cancellationToken = default);
        Task AddManyAsync(IEnumerable<Parameter> parameter, CancellationToken cancellationToken = default);    
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid parameterId, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] parameterIds);
        Task<IEnumerable<Parameter>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Parameter?> GetAsync(Guid parameterId, CancellationToken cancellationToken = default);
    }
}
