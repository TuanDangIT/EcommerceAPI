using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Mappings;
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
        public async Task<IEnumerable<PaymentDto>> BrowseAsync()
            => await _dbContext.Payments.Select(p => p.AsDto()).ToListAsync();
    }
}
