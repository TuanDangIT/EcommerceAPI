﻿using Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.UpdateProduct
{
    internal class UpdateProductValidator : AbstractValidator<UpdateProduct>
    {
        public UpdateProductValidator()
        {
        }
    }
}
