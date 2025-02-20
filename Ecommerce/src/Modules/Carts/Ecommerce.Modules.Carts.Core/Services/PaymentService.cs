using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly ICartsDbContext _dbContext;
        private readonly ILogger<PaymentService> _logger;
        private readonly IContextService _contextService;

        public PaymentService(ICartsDbContext dbContext, ILogger<PaymentService> logger, IContextService contextService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task SetActiveAsync(Guid paymentId, bool isActive, CancellationToken cancellationToken = default)
        {
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? throw new PaymentNotFoundException(paymentId);
            payment.SetActive(isActive);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Payment: {paymentId} was set to {isActive} by {@user}.", 
                paymentId, isActive, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        public async Task<IEnumerable<PaymentDto>> BrowseAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Payments
                .Select(p => p.AsDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        public async Task<IEnumerable<PaymentDto>> BrowseAvailableAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Payments
                .Where(p => p.IsActive == true)
                .Select(p => p.AsDto())
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }
}
