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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.DeleteSelectedCategories
{
    internal sealed class DeleteSelectedCategoriesHandler : ICommandHandler<DeleteSelectedCategories>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DeleteSelectedCategoriesHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteSelectedCategoriesHandler(ICategoryRepository categoryRepository, ILogger<DeleteSelectedCategoriesHandler> logger,
            IContextService contextService)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteSelectedCategories request, CancellationToken cancellationToken)
        {
            await _categoryRepository.DeleteManyAsync(request.CategoryIds, cancellationToken);
            _logger.LogInformation("Selected categories: {@categoryIds} were deleted by {@user}.",
                request.CategoryIds, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
