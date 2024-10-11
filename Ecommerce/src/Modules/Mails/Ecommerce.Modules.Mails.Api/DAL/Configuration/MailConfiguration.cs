using Ecommerce.Modules.Mails.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DAL.Configuration
{
    internal class MailConfiguration : IEntityTypeConfiguration<Mail>
    {
        public void Configure(EntityTypeBuilder<Mail> builder)
        {
            builder.Property(m => m.From).IsRequired();
            builder.Property(m => m.To).IsRequired();
            builder.Property(m => m.Subject).IsRequired();
            builder.Property(m => m.Body).IsRequired();
        }
    }
}
