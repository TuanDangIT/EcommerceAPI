using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public ChangeCategoryNameHandler(ICategoryRepository categoryRepository, TimeProvider timeProvider)
        {
            _categoryRepository = categoryRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(ChangeCategoryName request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.CategoryId);
            if (category is null)
            {
                throw new CategoryNotFoundException(request.CategoryId);
            }
            category.ChangeName(request.Name, _timeProvider.GetUtcNow().UtcDateTime);
            var rowsChanged = await _categoryRepository.UpdateAsync(category);
            if (rowsChanged != 1)
            {
                throw new CategoryNotUpdatedException(request.CategoryId);
            }
        }
    }
}
