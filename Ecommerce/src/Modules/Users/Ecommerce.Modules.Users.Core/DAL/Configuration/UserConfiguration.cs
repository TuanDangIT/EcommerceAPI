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

namespace Ecommerce.Modules.Users.Core.DAL.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Role).IsRequired();
            //builder.Property(x => x.CreatedAt)
            //    .HasColumnType("timestamp without time zone");
            //builder.Property(x => x.LastUpdatedAt)
            //    .HasColumnType("timestamp without time zone");
            builder.OwnsOne(x => x.RefreshToken);
        }
    }
}
