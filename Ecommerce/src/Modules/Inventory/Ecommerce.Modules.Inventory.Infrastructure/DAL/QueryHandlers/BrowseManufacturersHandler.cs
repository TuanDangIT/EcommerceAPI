using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.BrowseManufacturers;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class BrowseManufacturersHandler : IQueryHandler<BrowseManufacturers, IEnumerable<ManufacturerBrowseDto>>
    {
        public Task<IEnumerable<ManufacturerBrowseDto>> Handle(BrowseManufacturers request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
