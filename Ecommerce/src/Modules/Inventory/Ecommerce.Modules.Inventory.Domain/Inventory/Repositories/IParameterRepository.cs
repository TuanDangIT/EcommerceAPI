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
        Task AddAsync(Parameter parameter);
        Task UpdateAsync();
        Task DeleteAsync(Guid parameterId);
        Task DeleteManyAsync(Guid[] parameterIds);
        //Task<IEnumerable<Parameter>> GetAllAsync();
        Task<Parameter?> GetAsync(Guid parameterId);
    }
}
