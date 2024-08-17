using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.ApiResponse
{
    internal class ApiResponse
    {
        public int Code { get; set; }
        public string Status { get; set; } = string.Empty;
        public object[]? Data { get; set; }
        public DateTime Date {  get; set; } = TimeProvider.System.GetUtcNow().UtcDateTime;
    }
}
