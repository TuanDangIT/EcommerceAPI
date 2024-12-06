using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
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

        public PaymentService(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetActiveAsync(Guid paymentId, bool isActive, CancellationToken cancellationToken = default)
        {
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? throw new PaymentNotFoundException(paymentId);
            payment.SetActive(isActive);
            await _dbContext.SaveChangesAsync(cancellationToken);
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
