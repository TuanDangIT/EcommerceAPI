using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Parameters.BrowseParameters;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseParametersHandler : IQueryHandler<BrowseParameters, IEnumerable<ParameterBrowseDto>>
    {
        public Task<IEnumerable<ParameterBrowseDto>> Handle(BrowseParameters request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
