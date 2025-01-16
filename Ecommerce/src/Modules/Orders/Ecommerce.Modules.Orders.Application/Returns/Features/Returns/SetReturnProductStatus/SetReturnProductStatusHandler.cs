using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductStatus
{
    internal class SetReturnProductStatusHandler : ICommandHandler<SetReturnProductStatus>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly ILogger<SetReturnProductStatusHandler> _logger;
        private readonly IContextService _contextService;

        public SetReturnProductStatusHandler(IReturnRepository returnRepository, ILogger<SetReturnProductStatusHandler> logger,
            IContextService contextService)
        {
            _returnRepository = returnRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(SetReturnProductStatus request, CancellationToken cancellationToken)
        {
            if (Enum.TryParse(typeof(ReturnProductStatus), request.Status, true, out var status))
            {
                throw new ReturnInvalidReturnProductStatusException(request.Status);
            }
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken) ??
                throw new ReturnNotFoundException(request.ReturnId);
            @return.SetProductStatus(request.ProductId, (ReturnProductStatus)status!);
            await _returnRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Return's: {returnId} status was changed to {status} by {@user}.", @return.Id,
                request.Status, new { _contextService.Identity!.Username, _contextService.Identity.Id });
        }
    }
}
