﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteParameter
{
    internal class DeleteParameterValidator : AbstractValidator<DeleteParameter>
    {
        public DeleteParameterValidator()
        {
            RuleFor(d => d.ParameterId)
                .NotEmpty()
                .NotNull();
        }
    }
}
