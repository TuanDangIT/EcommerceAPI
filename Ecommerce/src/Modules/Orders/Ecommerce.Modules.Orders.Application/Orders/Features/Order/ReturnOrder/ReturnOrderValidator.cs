using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder
{
    internal sealed class ReturnOrderValidator : AbstractValidator<ReturnOrder>
    {
        public ReturnOrderValidator()
        {

        }
    }
}
