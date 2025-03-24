using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.GetComplaint;
using Ecommerce.Modules.Orders.Domain.Complaints.Repositories;
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
    internal sealed class GetComplaintHandler : IQueryHandler<GetComplaint, ComplaintDetailsDto?>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IContextService _contextService;

        public GetComplaintHandler(OrdersDbContext dbContext, IContextService contextService)
        {
            _dbContext = dbContext;
            _contextService = contextService;
        }
        public async Task<ComplaintDetailsDto?> Handle(GetComplaint request, CancellationToken cancellationToken)
            => await _dbContext.Complaints
                .AsNoTracking()
                .Include(c => c.Order)
                .ThenInclude(o => o.Customer)
                .Where(c => c.Id == request.ComplaintId)
                .Select(c => c.AsDetailsDto(_contextService.Identity == null || _contextService.Identity.Id == Guid.Empty || _contextService.Identity.Role == "Customer"))
                .FirstOrDefaultAsync(cancellationToken);
    }
}
