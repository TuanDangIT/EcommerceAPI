using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.ChangeParameterName
{
    public sealed record class ChangeParameterName(Guid ParameterId, string Name) : Shared.Abstractions.MediatR.ICommand;
}
