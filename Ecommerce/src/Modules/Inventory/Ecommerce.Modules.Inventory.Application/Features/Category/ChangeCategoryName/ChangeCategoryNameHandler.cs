using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.ChangeCategoryName
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
            if (rowsChanged is not 1)
            {
                throw new CategoryNotUpdatedException(request.CategoryId);
            }
        }
    }
}
