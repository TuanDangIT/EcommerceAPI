using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public interface IExceptionToResponseMapper
    {
        ProblemDetails Map(Exception exception);
    }
}
