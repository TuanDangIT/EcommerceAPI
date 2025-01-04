using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Exceptions
{
    internal class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;
        private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
        private readonly TimeProvider _timeProvider;
        private readonly IContextService _contextService;

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger, IExceptionToResponseMapper exceptionToResponseMapper, TimeProvider timeProvider, IContextService contextService)
        {
            _logger = logger;
            _exceptionToResponseMapper = exceptionToResponseMapper;
            _timeProvider = timeProvider;
            _contextService = contextService;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(_contextService.Identity is null)
            {
                _logger.LogError(exception, "An exception occured at {now}.", _timeProvider.GetUtcNow().UtcDateTime);
            }
            else
            {
                _logger.LogError(exception, "An exception occured at {now} for user: {@user}.", _timeProvider.GetUtcNow().UtcDateTime, new { _contextService.Identity.Username, _contextService.Identity.Id });
            }
            var response = _exceptionToResponseMapper.Map(exception);
            if(response is ValidationProblemDetails validationProblemDetails)
            {
                httpContext.Response.StatusCode = validationProblemDetails.Status ?? throw new ArgumentNullException();
                await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
                return true;
            }
            response.Extensions = new Dictionary<string, object?>()
            {
                { "traceId", httpContext.TraceIdentifier }
            }; 
            httpContext.Response.StatusCode = (int)response.Status!;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;

        }
    }
}
