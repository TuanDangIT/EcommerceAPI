using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Tests
{
    public class ValidationExceptionResponseTest
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public HttpStatusCode Status { get; set; }
    }
}
