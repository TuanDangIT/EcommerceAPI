using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public record ExceptionResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Status { get; set; } = string.Empty; 
        public string Message { get; set; } = string.Empty;
        public Error[]? Errors { get; set; }
        public DateTime Date { get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
        public ExceptionResponse(HttpStatusCode code, string status, string message, Error[] errors = default!)
        {
            Code = code;
            Status = status;
            Message = message;
            Errors = errors;
        }
    }
}
