using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.GetReturn;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class GetReturnHandler : IQueryHandler<GetReturn, ReturnDetailsDto?>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IContextService _contextService;

        public GetReturnHandler(OrdersDbContext dbContext, IContextService contextService)
        {
            _dbContext = dbContext;
            _contextService = contextService;
        }
        public async Task<ReturnDetailsDto?> Handle(GetReturn request, CancellationToken cancellationToken)
            => await _dbContext.Returns
                .AsNoTracking()
                .Include(r => r.Order)
                .ThenInclude(o => o.Customer)
                .Where(r => r.Id == request.ReturnId)
                .Select(r => r.AsDetailsDto(_contextService.Identity == null || _contextService.Identity.Id == Guid.Empty || _contextService.Identity.Role == "Customer"))
                .FirstOrDefaultAsync(cancellationToken);
    }
}
