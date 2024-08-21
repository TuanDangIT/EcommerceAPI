using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.ApiResponse
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Status { get; set; } = string.Empty;
        public object[]? Data { get; set; }
        public DateTime Date {  get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
        public ApiResponse(int code, string status, object[]? data = null)
        {
            Code = code;
            Status = status;
        }
    }
}
