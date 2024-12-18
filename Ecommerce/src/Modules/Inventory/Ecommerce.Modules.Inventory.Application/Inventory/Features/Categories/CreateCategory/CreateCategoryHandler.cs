using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.CreateCategory
{
    internal sealed class CreateCategoryHandler : ICommandHandler<CreateCategory>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CreateCategoryHandler> _logger;
        private readonly IContextService _contextService;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryHandler> logger,
            IContextService contextService)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Name);
            await _categoryRepository.AddAsync(category, cancellationToken);
            _logger.LogInformation("Category: {@category} was created by {@user}.", 
                category, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
