using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Exceptions
{
    internal class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                ValidationException ex => new ExceptionResponse(HttpStatusCode.BadRequest, "error", ex.Message, ex.Errors),
                EcommerceException ex => new ExceptionResponse(HttpStatusCode.BadRequest, "error", ex.Message),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "error", "There was a server error.")
            };
    }
}
