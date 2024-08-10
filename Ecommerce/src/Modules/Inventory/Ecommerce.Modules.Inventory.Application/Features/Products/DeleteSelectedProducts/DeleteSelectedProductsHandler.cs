using Ecommerce.Modules.Inventory.Application.Exceptions;
using Ecommerce.Modules.Inventory.Application.Features.Manufacturers.DeleteSelectedManufacturers;
using Ecommerce.Modules.Inventory.Domain.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.DeleteSelectedProducts
{
    internal sealed class DeleteSelectedProductsHandler : ICommandHandler<DeleteSelectedProducts>
    {
        private readonly IProductRepository _productRepository;

        public DeleteSelectedProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Handle(DeleteSelectedProducts request, CancellationToken cancellationToken)
        {
            var rowsChanged = await _productRepository.DeleteManyAsync(request.ProductIds);
            if (rowsChanged != request.ProductIds.Count())
            {
                throw new ManufacturerNotAllDeletedException();
            }
        }
    }
}
