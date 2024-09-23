using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services.Externals
{
    internal interface IStripeService
    {
        Task<(CheckoutStripeSessionDto Dto, string PaymentIntendId)> Checkout(CheckoutCart checkoutCart);
    }
}
