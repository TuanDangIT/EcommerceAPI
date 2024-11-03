using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.ChangeManufacturerName
{
    internal sealed class ChangeManufacturerNameValidator : AbstractValidator<ChangeManufacturerName>
    {
        public ChangeManufacturerNameValidator()
        {
            RuleFor(c => c.ManufaturerId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(32);
        }
    }
}
