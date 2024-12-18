using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.DeleteCategory
{
    internal sealed class DeleteCategoryHandler : ICommandHandler<DeleteCategory>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DeleteCategoryHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository, ILogger<DeleteCategoryHandler> logger,
            IContextService contextService)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            await _categoryRepository.DeleteAsync(request.CategoryId, cancellationToken);
            _logger.LogInformation("Category: {categoryId} was deleted by {@user}.", request.CategoryId, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
