using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Repositories
{
    public interface IParameterRepository
    {
        Task<int> AddAsync(Parameter parameter);
        Task<int> UpdateAsync(Parameter parameter);
        Task<int> DeleteAsync(Guid parameterId);
        Task<int> DeleteManyAsync(params Guid[] parameterIds);
        //Task<IEnumerable<Parameter>> GetAllAsync();
    }
}
