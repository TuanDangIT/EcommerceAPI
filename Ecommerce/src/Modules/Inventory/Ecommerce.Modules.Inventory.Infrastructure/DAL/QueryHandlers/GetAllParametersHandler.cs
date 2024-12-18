using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.GetAllParameters;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetAllParametersHandler : IQueryHandler<GetAllParameters, IEnumerable<ParameterOptionDto>>
    {
        private readonly InventoryDbContext _dbContext;

        public GetAllParametersHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ParameterOptionDto>> Handle(GetAllParameters request, CancellationToken cancellationToken)
            => await _dbContext.Parameters.Select(p => p.AsOptionDto()).ToListAsync(cancellationToken);
    }
}
