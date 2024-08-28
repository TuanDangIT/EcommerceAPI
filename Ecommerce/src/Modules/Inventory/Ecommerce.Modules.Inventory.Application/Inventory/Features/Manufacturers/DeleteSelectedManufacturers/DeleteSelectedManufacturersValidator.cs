using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteSelectedManufacturers
{
    internal sealed class DeleteSelectedManufacturersValidator : AbstractValidator<DeleteSelectedManufacturers>
    {
        public DeleteSelectedManufacturersValidator()
        {
            RuleFor(c => c.ManufacturerIds)
                .NotEmpty()
                .NotNull();
        }
    }
}
