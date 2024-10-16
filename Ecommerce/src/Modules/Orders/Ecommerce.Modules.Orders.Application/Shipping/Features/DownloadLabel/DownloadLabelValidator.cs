using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Shipping.Features.DownloadLabel
{
    internal class DownloadLabelValidator : AbstractValidator<DownloadLabel>
    {
        public DownloadLabelValidator()
        {
            RuleFor(d => d.ShipmentId)
                .NotEmpty()
                .NotNull();
        }
    }
}
