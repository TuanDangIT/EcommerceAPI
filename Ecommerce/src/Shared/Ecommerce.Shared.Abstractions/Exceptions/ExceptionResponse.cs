using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public record ExceptionResponse(Error Response, HttpStatusCode StatusCode);
}
