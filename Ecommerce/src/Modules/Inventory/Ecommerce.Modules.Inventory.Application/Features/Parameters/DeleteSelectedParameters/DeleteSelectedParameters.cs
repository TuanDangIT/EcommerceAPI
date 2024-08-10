using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Parameters.DeleteSelectedParameters
{
    public sealed record class DeleteSelectedParameters(Guid[] ParameterIds) : Shared.Abstractions.MediatR.ICommand;
}
