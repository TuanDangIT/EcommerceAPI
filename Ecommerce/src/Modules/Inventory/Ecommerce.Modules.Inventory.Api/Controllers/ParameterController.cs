using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Api.Controllers
{
    internal class ParameterController : BaseController
    {
        public ParameterController(IMediator mediator) : base(mediator)
        {
        }
    }
}
