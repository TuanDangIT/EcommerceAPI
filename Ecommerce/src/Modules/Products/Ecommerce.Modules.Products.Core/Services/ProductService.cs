using Ecommerce.Modules.Products.Core.DAL;
using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using Ecommerce.Modules.Products.Core.Exceptions;
using Ecommerce.Modules.Products.Core.Services.Mappings;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
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
        private readonly ISieveProcessor _sieveProcessor;

        public ProductService(IProductDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
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
        //public async Task<IEnumerable<ProductBrowseDto>> GetAllAsync(SieveModel sieveModel)
        //    => await _dbContext.Products.Select(p => p.AsBrowseDto()).ToListAsync();
        public async Task<PagedResult<ProductBrowseDto>> BrowseProductsAsync(SieveModel sieveModel)
        {
            var products = _dbContext.Products
                .AsNoTracking()
                .AsQueryable();
            var dtos = await _sieveProcessor
                .Apply(sieveModel, products)
                .Select(r => r.AsBrowseDto())
                .ToListAsync();
            var totalCount = await _sieveProcessor
                .Apply(sieveModel, products, applyPagination: false)
                .CountAsync();
            if (sieveModel.PageSize is null || sieveModel.Page is null)
            {
                throw new PaginationException();
            }
            var pagedResult = new PagedResult<ProductBrowseDto>(dtos, totalCount, sieveModel.PageSize.Value, sieveModel.Page.Value);
            return pagedResult;
        }
        public async Task<ProductDetailsDto?> GetAsync(Guid productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.Reviews)
                .SingleOrDefaultAsync(p => p.Id == productId);
            return product?.AsDetailsDto();
        }

    }
}
