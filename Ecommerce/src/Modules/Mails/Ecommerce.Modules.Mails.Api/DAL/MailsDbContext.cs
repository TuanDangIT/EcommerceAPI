﻿using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Shared.Abstractions.Entities;
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
        private const string _schema = "mails";
        private readonly TimeProvider _timeProvider;

        public MailsDbContext(DbContextOptions<MailsDbContext> options, TimeProvider timeProvider) : base(options)
        {
            _timeProvider = timeProvider;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Mail>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Mail.CreatedAt)).CurrentValue = _timeProvider.GetUtcNow().UtcDateTime;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public Task<int> SaveChangesAsync()
            => SaveChangesAsync(default);
    }
}
