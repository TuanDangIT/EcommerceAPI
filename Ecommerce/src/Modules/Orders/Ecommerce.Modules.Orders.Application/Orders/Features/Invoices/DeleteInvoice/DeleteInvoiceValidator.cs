using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DeleteInvoice
{
    internal class DeleteInvoiceValidator : AbstractValidator<DeleteInvoice>
    {
        public DeleteInvoiceValidator()
        {
            RuleFor(d => d.OrderId)
                .NotNull()
                .NotEmpty();
        }
    }
}
