using Ecommerce.Modules.Carts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    internal class CheckoutCartController
    {
        private readonly ICheckoutCartService _checkoutCartService;

        public CheckoutCartController(ICheckoutCartService checkoutCartService)
        {
            _checkoutCartService = checkoutCartService;
        }
    }
}
