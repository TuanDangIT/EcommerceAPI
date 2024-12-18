using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.GetAllManufacturers;
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
    internal sealed class GetAllManufacturersHandler : IQueryHandler<GetAllManufacturers, IEnumerable<ManufacturerOptionDto>>
    {
        private readonly InventoryDbContext _dbContext;

        public GetAllManufacturersHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ManufacturerOptionDto>> Handle(GetAllManufacturers request, CancellationToken cancellationToken)
            => await _dbContext.Manufacturers.Select(m => m.AsOptionDto()).ToListAsync(cancellationToken);
    }
}
