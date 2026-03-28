using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Orders.BrowseProductsToAdd;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseProductsToAddHandler : ICommandHandler<BrowseProductsToAdd, IEnumerable<ProductToAddBrowseDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IContextService _contextService;
        private readonly ILogger<BrowseProductsToAdd> _logger;
        private const int _maxResults = 20;

        public BrowseProductsToAddHandler(OrdersDbContext dbContext, IContextService contextService, ILogger<BrowseProductsToAdd> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<IEnumerable<ProductToAddBrowseDto>> Handle(BrowseProductsToAdd request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products
                .Where(p => p.Name.Contains(request.SearchTerm) || p.SKU.Contains(request.SearchTerm))
                .Select(p => new ProductToAddBrowseDto
                {
                    Id = p.Id,
                    SKU = p.SKU,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    ImagePathUrl = p.ImagePathUrl
                })
                .Take(_maxResults)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("User {UserId} searched for products to add to order {OrderId} with search term '{SearchTerm}' and found {ProductCount} products.",
                _contextService.Identity!.Id, request.OrderId, request.SearchTerm, products.Count);   

            return products;
        }
    }
}
