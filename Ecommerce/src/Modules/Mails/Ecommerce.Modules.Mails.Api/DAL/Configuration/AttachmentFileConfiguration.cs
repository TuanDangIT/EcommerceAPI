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
    internal class AttachmentFileConfiguration : IEntityTypeConfiguration<AttachmentFile>
    {
        public void Configure(EntityTypeBuilder<AttachmentFile> builder)
        {
            builder.Property(af => af.Id)
                .HasConversion(v => v.ToString(), v => Ulid.Parse(v));
            builder.Property(af => af.FileName)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
