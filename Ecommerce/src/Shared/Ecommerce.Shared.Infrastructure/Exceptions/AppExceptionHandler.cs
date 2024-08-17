using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger, IExceptionToResponseMapper exceptionToResponseMapper, TimeProvider timeProvider)
        {
            _logger = logger;
            _exceptionToResponseMapper = exceptionToResponseMapper;
            _timeProvider = timeProvider;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"Exception occured at {_timeProvider.GetUtcNow().UtcDateTime
                }: {exception.Message}");
            var response = _exceptionToResponseMapper.Map(exception);
            httpContext.Response.StatusCode = (int)response.Code;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;

        }
    }
}
