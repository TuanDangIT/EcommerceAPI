using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateInvoice
{
    internal class CreateInvoiceValidator : AbstractValidator<CreateInvoice>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(c => c.OrderId)
                .NotNull()
                .NotEmpty();
        }
    }
}
