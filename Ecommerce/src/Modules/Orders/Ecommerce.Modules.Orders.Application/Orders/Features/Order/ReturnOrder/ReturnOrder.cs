using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ReturnOrder
{
    //[ModelBinder(BinderType = typeof(SwaggerArrayBinder))]
    public sealed record class ReturnOrder(string ReasonForReturn, IEnumerable<ProductToReturnDto> ProductsToReturn, Guid OrderId) : ICommand;
}
