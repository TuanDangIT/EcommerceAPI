﻿using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.HandleOrderShipped
{
    public sealed record class HandlerOrderShipped(string Json) : ICommand;
}
