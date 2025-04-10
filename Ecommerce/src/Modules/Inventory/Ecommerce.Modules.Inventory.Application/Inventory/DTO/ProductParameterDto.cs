﻿using Ecommerce.Shared.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.DTO
{
    [ModelBinder(BinderType = typeof(ListModelBinder))]
    public class ProductParameterDto
    {
        public Guid ParameterId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
