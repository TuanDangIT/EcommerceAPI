using Ecommerce.Modules.Orders.Application.Returns.DTO;
using Ecommerce.Modules.Orders.Application.Returns.Features.Return.GetReturn;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class GetReturnHandler : IQueryHandler<GetReturn, ReturnDetailsDto?>
    {
        private readonly IReturnRepository _returnRepository;

        public GetReturnHandler(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }
        public async Task<ReturnDetailsDto?> Handle(GetReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId);
            return @return?.AsDetailsDto();
        }
    }
}
