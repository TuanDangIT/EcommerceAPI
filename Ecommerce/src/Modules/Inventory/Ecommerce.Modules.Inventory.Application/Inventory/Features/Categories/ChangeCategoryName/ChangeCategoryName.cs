using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.ChangeCategoryName
{
    public sealed record class ChangeCategoryName(string Name) : ICommand
    {
        [SwaggerIgnore]
        public Guid CategoryId { get; init; }
    }
}
