using Ecommerce.Modules.Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL
{
    internal interface ICartsDbContext
    {
        DbSet<Cart> Carts { get; set; }
        DbSet<CheckoutCart> CheckoutCarts { get; set; }
        DbSet<CartProduct> CartProducts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Discount> Discounts { get; set; }
        Task<int> SaveChangesAsync();
    }
}
