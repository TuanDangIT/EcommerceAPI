using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IContextService _contextService;
        private const string BadRequestTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        private const string ServerErrorTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
        public ExceptionToResponseMapper(IContextService contextService)
        {
            _contextService = contextService;
        }
        public ProblemDetails Map(Exception exception)
            => exception switch
            {
                ValidationException ex => new ValidationProblemDetails()
                {
                    Type = BadRequestTypeUrl,
                    Title = ex.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = ex.Errors,
                },
                EcommerceException ex => new ProblemDetails()
                {
                    Type = BadRequestTypeUrl,
                    Title = ex.Message,
                    Status = (int)HttpStatusCode.BadRequest,
                },
                _ => new ProblemDetails()
                {
                    Type = ServerErrorTypeUrl,
                    Title = "There was a server error",
                    Status = (int)HttpStatusCode.InternalServerError,

                }
            };
    }
}
