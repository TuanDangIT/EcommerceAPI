using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using Ecommerce.Modules.Products.Core.Services.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductDbContext _dbContext;

        public ProductService(IProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Te metody z komentarza będą później użyte jako external dla event z inventory
        //public async Task<int> AddAsync(Product product)
        //{
        //    await _dbContext.Products.AddAsync(product);
        //    return await _dbContext.SaveChangesAsync();
        //}
        //public async Task<int> DeleteAsync(Guid productId) 
        //    => await _dbContext.Products.Where(p => p.Id == productId).ExecuteDeleteAsync();
        //public async Task<int> DeleteManyAsync(Guid[] productIds) 
        //    => await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ExecuteDeleteAsync();
        //public async Task<int> UpdateAsync(Product product)
        //{
        //    _dbContext.Products.Update(product);
        //    return await _dbContext.SaveChangesAsync();
        //}
        public async Task<IEnumerable<ProductBrowseDto>> GetAllAsync()
            => await _dbContext.Products.Select(p => p.AsBrowseDto()).ToListAsync();
        public async Task<ProductDetailsDto?> GetAsync(Guid productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.Reviews)
                .SingleOrDefaultAsync(p => p.Id == productId);
            return product?.AsDetailsDto();
        }

    }
}
