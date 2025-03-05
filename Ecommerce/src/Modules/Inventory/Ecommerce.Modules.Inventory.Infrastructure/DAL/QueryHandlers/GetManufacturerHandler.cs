using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.GetManufacturer;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;

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
            => await _dbContext.Manufacturers
                .AsNoTracking()
                .Where(m => m.Id == request.ManufacturerId)
                .Select(m => m.AsBrowseDto())
                .FirstOrDefaultAsync(cancellationToken);
    }
}
