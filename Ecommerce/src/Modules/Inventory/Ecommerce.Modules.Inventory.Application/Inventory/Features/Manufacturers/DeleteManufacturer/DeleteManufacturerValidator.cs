using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Manufacturers.DeleteManufacturer
{
    internal class DeleteManufacturerValidator : AbstractValidator<DeleteManufacturer>
    {
        public DeleteManufacturerValidator()
        {
            RuleFor(d => d.ManufacturerId)
                .NotNull()
                .NotEmpty();
        }
    }
}
