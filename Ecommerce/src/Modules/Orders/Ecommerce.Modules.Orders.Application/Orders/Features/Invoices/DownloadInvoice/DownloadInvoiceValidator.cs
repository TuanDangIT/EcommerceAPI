using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DownloadInvoice
{
    internal class DownloadInvoiceValidator : AbstractValidator<DownloadInvoice>
    {
        public DownloadInvoiceValidator()
        {
            RuleFor(d => d.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
