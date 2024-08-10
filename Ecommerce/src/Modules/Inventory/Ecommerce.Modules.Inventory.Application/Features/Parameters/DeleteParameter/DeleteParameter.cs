using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteParameter
{
    public sealed record class DeleteParameter(Guid ParameterId) : Shared.Abstractions.MediatR.ICommand;
}
