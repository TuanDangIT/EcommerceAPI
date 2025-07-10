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
    internal class DeliveryServiceService : IDeliveryServiceService
    {
        private readonly ICartsDbContext _dbContext;

        public DeliveryServiceService(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DeliveryServiceDto>> GetAllAsync(bool? isActive, CancellationToken cancellationToken = default)
            => await _dbContext.DeliveryServices
            .Where(ds => ds.IsActive == isActive)
            .Select(ds => ds.AsDto())
            .ToListAsync(cancellationToken);

        public async Task SetActiveAsync(int deliveryServiceId, bool isActive, CancellationToken cancellationToken = default)
        {
            var deliveryService = await _dbContext.DeliveryServices
                .FirstOrDefaultAsync(ds => ds.Id == deliveryServiceId, cancellationToken) ?? throw new DeliveryServiceNotFoundException(deliveryServiceId);

            deliveryService.SetActive(isActive);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
