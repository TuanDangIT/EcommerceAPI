using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Api
{
    public class ApiResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Status { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Date {  get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
        public ApiResponse(HttpStatusCode code, string status, T? data = default)
        {
            Code = code;
            Status = status;
            Data = data;
        }
    }
}
