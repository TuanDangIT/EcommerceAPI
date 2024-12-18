using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.GetAuction;
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
    internal class GetAuctionHandler : IQueryHandler<GetAuction, AuctionDetailsDto?>
    {
        private readonly InventoryDbContext _dbContext;

        public GetAuctionHandler(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AuctionDetailsDto?> Handle(GetAuction request, CancellationToken cancellationToken)
            => await _dbContext.Auctions
                .AsNoTracking()
                .Include(a => a.Reviews)
                .Where(a => a.Id == request.AuctionId)
                .Select(a => a.AsDetailsDto())
                .SingleOrDefaultAsync(cancellationToken);
    }
}
