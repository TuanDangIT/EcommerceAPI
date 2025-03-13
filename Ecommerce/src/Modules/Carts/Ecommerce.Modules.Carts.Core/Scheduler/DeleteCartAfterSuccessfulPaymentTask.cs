using Coravel.Invocable;
using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Scheduler
{
    internal class DeleteCartAfterSuccessfulPaymentTask : IInvocable
    {
        private readonly ICartsDbContext _dbContext;
        private readonly ILogger<DeleteCartAfterSuccessfulPaymentTask> _logger;
        private readonly Guid _cartId;

        public DeleteCartAfterSuccessfulPaymentTask(ICartsDbContext dbContext, ILogger<DeleteCartAfterSuccessfulPaymentTask> logger, Guid cartId)
        {
            _dbContext = dbContext;
            _logger = logger;
            _cartId = cartId;
        }
        public async Task Invoke()
        {
            await Task.Delay(5000);
            await _dbContext.Carts
                .Where(c => c.Id == _cartId)
                .ExecuteDeleteAsync();
            _logger.LogDebug($"Cart: {_cartId} was deleted.");
        }
    }
}
