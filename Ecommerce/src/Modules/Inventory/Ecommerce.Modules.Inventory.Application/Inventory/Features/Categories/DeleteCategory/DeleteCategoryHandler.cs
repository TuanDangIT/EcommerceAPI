using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Handle(DeleteCategory request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _categoryRepository.DeleteAsync(request.CategoryId);
            if (rowsChanged != 1)
            {
                throw new CategoryNotDeletedException(request.CategoryId);
            }
        }
    }
}
