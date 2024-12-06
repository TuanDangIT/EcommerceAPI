using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services.Externals
{
    internal interface IPaymentProcessorService
    {
        Task<CheckoutStripeSessionDto> CheckoutAsync(CheckoutCart checkoutCart, CancellationToken cancellationToken = default);
    }
}
