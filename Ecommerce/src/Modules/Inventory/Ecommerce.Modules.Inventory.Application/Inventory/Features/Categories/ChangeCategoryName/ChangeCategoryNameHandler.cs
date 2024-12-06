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

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Categories.ChangeCategoryName
{
    internal sealed class ChangeCategoryNameHandler : ICommandHandler<ChangeCategoryName>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ChangeCategoryNameHandler> _logger;
        private readonly IContextService _contextService;

        public ChangeCategoryNameHandler(ICategoryRepository categoryRepository, ILogger<ChangeCategoryNameHandler> logger,
            IContextService contextService)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(ChangeCategoryName request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.CategoryId);
            if (category is null)
            {
                throw new CategoryNotFoundException(request.CategoryId);
            }
            category.ChangeName(request.Name);
            await _categoryRepository.UpdateAsync();
            _logger.LogInformation("Category's: {category} name was changed to {newName} by {username}:{userId}.", category, request.Name,
                _contextService.Identity!.Username, _contextService.Identity!.Id);
        }
    }
}
