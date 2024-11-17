using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shared.Stripe
{
    public interface IStripeService
    {
        Task Refund(Order order);
        Task Refund(Order order, decimal amount);
    }
}
