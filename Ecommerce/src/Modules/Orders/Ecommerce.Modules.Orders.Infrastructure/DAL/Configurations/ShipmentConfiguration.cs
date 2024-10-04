using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configurations
{
    internal class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.OwnsOne(s => s.Receiver, r =>
            {
                r.OwnsOne(r => r.Address, a =>
                {
                    a.Property(a => a.Street)
                        .IsRequired();
                    a.Property(a => a.BuildingNumber)
                        .IsRequired();
                    a.Property(a => a.City)
                        .IsRequired();
                    a.Property(a => a.PostCode)
                        .IsRequired();
                    a.Property(a => a.CountryCode)
                        .IsRequired();
                });
                r.Property(r => r.FirstName)
                    .IsRequired();
                r.Property(r => r.LastName)
                    .IsRequired();
                r.Property(r => r.Email)
                    .IsRequired();
                r.Property(r => r.Phone)
                    .IsRequired();
            });
            builder.OwnsMany(r => r.Parcels, p =>
            {
                p.OwnsOne(p => p.Dimensions, d =>
                {
                    d.Property(d => d.Length)
                        .IsRequired();
                    d.Property(d => d.Width)
                        .IsRequired();
                    d.Property(d => d.Height)
                        .IsRequired();
                    d.Property(d => d.Unit)
                        .IsRequired();
                });
                p.OwnsOne(p => p.Weight, w =>
                {
                    w.Property(w => w.Amount)
                        .IsRequired();
                    w.Property(w => w.Unit)
                        .IsRequired();
                });
            });
            builder.OwnsOne(s => s.Insurance, i =>
            {
                i.Property(i => i.Amount)
                    .IsRequired();
                i.Property(i => i.Currency)
                    .IsRequired();
            });
            builder.Property(s => s.Service)
                .IsRequired();
        }
    }
}
