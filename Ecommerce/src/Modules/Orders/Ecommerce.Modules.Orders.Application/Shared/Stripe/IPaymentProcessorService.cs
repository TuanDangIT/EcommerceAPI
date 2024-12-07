using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shared.Stripe
{
    public interface IPaymentProcessorService
    {
        Task RefundAsync(Order order, CancellationToken cancellationToken = default);
        Task RefundAsync(Order order, decimal amount, CancellationToken cancellationToken = default);
    }
}
