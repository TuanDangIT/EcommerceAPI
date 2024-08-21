using Ecommerce.Shared.Infrastructure.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.DTO
{
    [ModelBinder(BinderType = typeof(SwaggerArrayBinder))]
    public class ProductParameterDto
    {
        public Guid ParameterId { get; set; }   
        public string Value { get; set; } = string.Empty;
    }
}
