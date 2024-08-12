﻿using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.DeleteManyCategories
{
    internal sealed class DeleteManyCategoriesHandler : ICommandHandler<DeleteManyCategories>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteManyCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Handle(DeleteManyCategories request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _categoryRepository.DeleteManyAsync(request.CategoryIds);
            if (rowsChanged != request.CategoryIds.Count())
            {
                throw new ParameterNotAllDeletedException();
            }
        }
    }
}
