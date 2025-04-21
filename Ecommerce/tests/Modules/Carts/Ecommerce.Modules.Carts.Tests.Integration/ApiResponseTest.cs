using Ecommerce.Modules.Carts.Tests.Integration.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Tests.Integration
{
    public class ApiResponseTest<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Status { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
    public class CreateCartData
    {
        public Guid Id { get; set; }
    }
    public class GetCartData
    {
        public Guid Id { get; set; }
    }
}
