using Ecommerce.Modules.Mails.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DAL
{
    internal class MailsDbContext : DbContext, IMailsDbContext
    {
        public DbSet<Customer> Customers { get; set; }  
        public DbSet<Mail> Mails { get; set; }
        private const string Schema = "mails";
        public MailsDbContext(DbContextOptions<MailsDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public Task<int> SaveChangesAsync()
            => base.SaveChangesAsync();
    }
}
