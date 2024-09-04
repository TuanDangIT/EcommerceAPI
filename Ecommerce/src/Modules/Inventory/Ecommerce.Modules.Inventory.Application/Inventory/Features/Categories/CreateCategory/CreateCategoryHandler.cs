using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, TimeProvider timeProvider)
        {
            _categoryRepository = categoryRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(CreateCategory request, CancellationToken cancellationToken)
        {
            await _categoryRepository.AddAsync(new Category(Guid.NewGuid(), request.Name, _timeProvider.GetUtcNow().UtcDateTime));
        }
    }
}
