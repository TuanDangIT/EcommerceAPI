using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName
{
    public sealed record class ChangeCategoryName(Guid CategoryId, string Name) : ICommand;
}
