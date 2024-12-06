using Ecommerce.Modules.Discounts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL
{
    internal interface IDiscountDbContext
    {
        DbSet<Offer> Offers { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<Coupon> Coupons { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
