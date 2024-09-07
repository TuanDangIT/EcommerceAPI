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
        public string Status { get; set; } = "success";
        public T? Data { get; set; }
        public DateTime Date {  get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
        public ApiResponse(HttpStatusCode code, string status, T? data = default) : this(code, data)
        {
            Status = status;
        }
        public ApiResponse(HttpStatusCode code, T? data = default)
        {
            Code = code;
            Data = data;
        }
    }
}
