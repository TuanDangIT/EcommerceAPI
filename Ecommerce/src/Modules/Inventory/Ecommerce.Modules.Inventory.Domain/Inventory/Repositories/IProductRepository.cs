﻿using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IProductRepository
    {
        Task<int> AddAsync(Product product);
        Task<int> UpdateAsync();
        Task<int> DeleteAsync(Guid productId);
        //Task<int> DecreaseQuantityAsync(Guid productId, int ammount);
        Task<int> DeleteManyAsync(Guid[] productIds);
        Task<IEnumerable<Guid>> GetAllIdThatContainsInArrayAsync(Guid[] productIds);
        Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds);
        Task<Product?> GetAsync(Guid productId);
        Task DeleteProductParametersAndImagesRelatedToProduct(Guid productId);
    }
}
