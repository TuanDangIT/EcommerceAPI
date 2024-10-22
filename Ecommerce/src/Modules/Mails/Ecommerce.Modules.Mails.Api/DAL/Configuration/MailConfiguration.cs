using Ecommerce.Modules.Mails.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            //builder.Property(m => m.AttachmentFileNames)
            //    .HasConversion(a => string.Join(',', a), a => a.Split(',', StringSplitOptions.None));
            //builder.Property(x => x.AttachmentFileNames).Metadata.SetValueComparer(
            //    new ValueComparer<IEnumerable<string>>(
            //        (a1, a2) => a1!.SequenceEqual(a2!),
            //        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));
        }
    }
}
