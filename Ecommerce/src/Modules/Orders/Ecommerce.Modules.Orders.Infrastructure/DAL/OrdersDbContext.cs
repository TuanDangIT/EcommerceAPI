using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL
{
    internal class OrdersDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<Complaint> Complaints { get; set; }   
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("orders");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
