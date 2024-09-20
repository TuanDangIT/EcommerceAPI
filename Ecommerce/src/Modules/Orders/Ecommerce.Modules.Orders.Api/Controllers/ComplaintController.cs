using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    internal class ComplaintController : BaseController
    {
        public ComplaintController(IMediator mediator) : base(mediator)
        {
        }
    }
}
