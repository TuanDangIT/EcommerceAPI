using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Exceptions
{
    internal class HttpClientRequestFailedException : EcommerceException
    {
        public HttpClientRequestFailedException(string message) : base($"A HTTP request that was sent failed. Message: {message}")
        {
        }
    }
}
