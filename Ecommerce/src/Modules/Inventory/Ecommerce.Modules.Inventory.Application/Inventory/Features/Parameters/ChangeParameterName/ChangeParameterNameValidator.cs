﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Parameters.ChangeParameterName
{
    internal sealed class ChangeParameterNameValidator : AbstractValidator<ChangeParameterName>
    {
        public ChangeParameterNameValidator()
        {
            RuleFor(c => c.ParameterId)
                .NotNull()
                .NotEmpty();
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(32);
        }
    }
}
