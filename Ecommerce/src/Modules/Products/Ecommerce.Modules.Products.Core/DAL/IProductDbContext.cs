using Ecommerce.Modules.Products.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.DAL
{
    internal interface IProductDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Review> Reviews { get; set; }
        Task<int> SaveChangesAsync();
    }
}
