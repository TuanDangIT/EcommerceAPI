﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.MediatR
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
