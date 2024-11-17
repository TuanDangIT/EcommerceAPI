using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation
{
    internal class WriteAdditionalInformationValidator : AbstractValidator<WriteAdditionalInformation>
    {
        public WriteAdditionalInformationValidator()
        {
            RuleFor(w => w.OrderId)
                .NotNull()
                .NotEmpty();
            RuleFor(w => w.AdditionalInformation)
                .NotNull();
        }
    }
}
