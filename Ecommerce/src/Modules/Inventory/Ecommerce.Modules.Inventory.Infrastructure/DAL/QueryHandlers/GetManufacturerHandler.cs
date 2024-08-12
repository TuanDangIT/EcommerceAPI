using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.GetManufacturer;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetManufacturerHandler : IQueryHandler<GetManufacturer, ManufacturerBrowseDto?>
    {
        private readonly InventoryDbContext _dbContext;

        public GetManufacturerHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ManufacturerBrowseDto?> Handle(GetManufacturer request, CancellationToken cancellationToken)
        {
            var manufacturer = await _dbContext.Manufacturers.AsNoTracking().SingleOrDefaultAsync(m => m.Id == request.ManufacturerId);
            return manufacturer?.AsBrowseDto();
        }
    }
}
