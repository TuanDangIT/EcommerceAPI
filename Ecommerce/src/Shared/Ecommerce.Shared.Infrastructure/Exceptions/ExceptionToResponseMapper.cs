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
                EcommerceException ex => new ExceptionResponse(new Error(ex.GetType().Name, ex.Message)
                    , HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new Error("error", "There was an error."),
                    HttpStatusCode.InternalServerError)
            };
    }
}
