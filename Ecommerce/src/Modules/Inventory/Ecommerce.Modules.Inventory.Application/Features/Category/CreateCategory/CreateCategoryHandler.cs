using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Category.CreateCategory
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
            var rowsChanged = await _categoryRepository.AddAsync(new Domain.Entities.Category(Guid.NewGuid(), request.Name, _timeProvider.GetUtcNow().UtcDateTime));
            if (rowsChanged is not 1)
            {
                throw new CategoryNotCreatedException();
            }
        }
    }
}
