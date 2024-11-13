﻿using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync();
        Task DeleteAsync(Guid productId);
        //Task<int> DecreaseQuantityAsync(Guid productId, int ammount);
        Task DeleteManyAsync(Guid[] productIds);
        Task<IEnumerable<Guid>> GetAllIdThatContainsInArrayAsync(Guid[] productIds);
        Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds);
        Task<Product?> GetAsync(Guid productId);
        Task DeleteProductParametersAndImagesRelatedToProduct(Guid productId);
        Task UpdateListedFlagAsync(Guid[] productIds, bool isListed);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
