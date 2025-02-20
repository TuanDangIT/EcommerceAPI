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
        private const string _badRequestTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        private const string _serverErrorTypeUrl = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
        public ExceptionToResponseMapper(IContextService contextService)
        {
            _contextService = contextService;
        }
        public ProblemDetails Map(Exception exception)
            => exception switch
            {
                ValidationException e => new ValidationProblemDetails()
                {
                    Type = _badRequestTypeUrl,
                    Title = "An validation exception occured.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = e.Errors,
                    Detail = e.Message
                },
                EcommerceException e => new ProblemDetails()
                {
                    Type = _badRequestTypeUrl,
                    Title = "An exception occured.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = e.Message,
                },
                Exception e => new ProblemDetails()
                {
                    Type = _serverErrorTypeUrl,
                    Title = "There was a server error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = e.Message
                }
            };
    }
}
