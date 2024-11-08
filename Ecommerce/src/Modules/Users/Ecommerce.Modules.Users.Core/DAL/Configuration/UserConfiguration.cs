using Ecommerce.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ecommerce.Modules.Users.Core.Entities.Enums;

namespace Ecommerce.Modules.Users.Core.DAL.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(64);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Password)
                .IsRequired();
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(16);
            builder.HasIndex(x => x.Username).IsUnique();
            //builder.Property(x => x.CreatedAt)
            //    .HasColumnType("timestamp without time zone");
            //builder.Property(x => x.LastUpdatedAt)
            //    .HasColumnType("timestamp without time zone");
            builder.OwnsOne(x => x.RefreshToken);
            //builder.Property(u => u.Role)
            //    .IsRequired();
            //builder.HasOne(u => u.Role)
            //    .WithMany(r => r.Users)
            //    .HasForeignKey(u => u.RoleId);
            builder.HasDiscriminator(u => u.Type)
                .HasValue<Employee>(UserType.Employee)
                .HasValue<Customer>(UserType.Customer);
            builder.Property(u => u.Type)
                .HasConversion<string>();
            builder.Property(u => u.FirstName)
                .HasMaxLength(64);
            builder.Property(u => u.LastName)
                .HasMaxLength(64);
            builder.Property(u => u.IsActive)
                .IsRequired();
        }
    }
}
