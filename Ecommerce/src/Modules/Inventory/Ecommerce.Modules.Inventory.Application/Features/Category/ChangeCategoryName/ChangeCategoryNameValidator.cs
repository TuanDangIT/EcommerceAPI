﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName
{
    internal class ChangeCategoryNameValidator : AbstractValidator<ChangeCategoryName>
    {
        public ChangeCategoryNameValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull();
        }
    }
}
