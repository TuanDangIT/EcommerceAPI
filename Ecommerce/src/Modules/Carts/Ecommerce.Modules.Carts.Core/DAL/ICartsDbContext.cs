using Ecommerce.Modules.Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Carts.Tests.Unit")]
namespace Ecommerce.Modules.Carts.Core.DAL
{
    public interface ICartsDbContext
    {
        DbSet<Cart> Carts { get; set; }
        DbSet<CheckoutCart> CheckoutCarts { get; set; }
        DbSet<CartProduct> CartProducts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Discount> Discounts { get; set; }
        DbSet<DeliveryService> DeliveryServices { get; set; }
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
