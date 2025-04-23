using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Tests
{
    public class ApiResponseTest<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Status { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
