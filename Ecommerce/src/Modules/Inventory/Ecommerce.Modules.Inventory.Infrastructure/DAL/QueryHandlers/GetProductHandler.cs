using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.GetProduct;
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
    internal sealed class GetProductHandler : IQueryHandler<GetProduct, ProductDetailsDto?>
    {
        private readonly InventoryDbContext _dbContext;

        public GetProductHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductDetailsDto?> Handle(GetProduct request, CancellationToken cancellationToken)
            => await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .Include(p => p.Images.OrderBy(i => i.Order))
                .Include(p => p.ProductParameters)
                .ThenInclude(pp => pp.Parameter)
                .Where(p => p.Id == request.ProductId)
                .Select(p => p.AsDetailsDto())
                .SingleOrDefaultAsync(cancellationToken);
    }
}
