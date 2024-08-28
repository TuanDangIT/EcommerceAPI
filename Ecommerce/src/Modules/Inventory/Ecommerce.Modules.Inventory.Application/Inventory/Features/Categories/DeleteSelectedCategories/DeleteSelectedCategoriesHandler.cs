using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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

        public DeleteSelectedCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Handle(DeleteSelectedCategories request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _categoryRepository.DeleteManyAsync(request.CategoryIds);
            if (rowsChanged != request.CategoryIds.Count())
            {
                throw new ParameterNotAllDeletedException();
            }
        }
    }
}
