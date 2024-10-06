﻿// <auto-generated />
using System;
using Ecommerce.Modules.Discounts.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Discounts.Core.DAL.Migrations
{
    [DbContext(typeof(DiscountsDbContext))]
    partial class DiscountsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("discounts")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("StripeCouponId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("StripeCouponId")
                        .IsUnique();

                    b.ToTable("Coupons", "discounts");

                    b.HasDiscriminator<string>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<int>("CouponId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("Redemptions")
                        .HasColumnType("integer");

                    b.Property<string>("StripePromotionCodeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CouponId");

                    b.HasIndex("StripePromotionCodeId")
                        .IsUnique();

                    b.ToTable("Discounts", "discounts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("OldPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Price")
                        .HasPrecision(11, 2)
                        .HasColumnType("numeric(11,2)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Offers", "discounts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.NominalCoupon", b =>
                {
                    b.HasBaseType("Ecommerce.Modules.Discounts.Core.Entities.Coupon");

                    b.Property<decimal>("NominalValue")
                        .HasPrecision(8, 2)
                        .HasColumnType("numeric(8,2)");

                    b.HasDiscriminator().HasValue("NominalCoupon");
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.PercentageCoupon", b =>
                {
                    b.HasBaseType("Ecommerce.Modules.Discounts.Core.Entities.Coupon");

                    b.Property<decimal>("Percent")
                        .HasPrecision(2, 2)
                        .HasColumnType("numeric(2,2)");

                    b.HasDiscriminator().HasValue("PercentageCoupon");
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.Discount", b =>
                {
                    b.HasOne("Ecommerce.Modules.Discounts.Core.Entities.Coupon", "Coupon")
                        .WithMany("Discounts")
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");
                });

            modelBuilder.Entity("Ecommerce.Modules.Discounts.Core.Entities.Coupon", b =>
                {
                    b.Navigation("Discounts");
                });
#pragma warning restore 612, 618
        }
    }
}
