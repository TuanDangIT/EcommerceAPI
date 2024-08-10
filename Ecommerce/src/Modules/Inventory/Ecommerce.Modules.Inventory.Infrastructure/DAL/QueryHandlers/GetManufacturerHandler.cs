using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.GetManufacturer;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.QueryHandlers
{
    internal sealed class GetManufacturerHandler : IQueryHandler<GetManufacturer, ManufacturerDto>
    {
        public Task<ManufacturerDto> Handle(GetManufacturer request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
