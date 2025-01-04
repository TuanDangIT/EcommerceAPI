﻿// <auto-generated />
using System;
using Ecommerce.Modules.Carts.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    [DbContext(typeof(CartsDbContext))]
    partial class CartsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("carts")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CheckoutCartId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CheckoutCartId")
                        .IsUnique();

                    b.HasIndex("DiscountId");

                    b.ToTable("Carts", "carts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CartProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CheckoutCartId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("DiscountedPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("CheckoutCartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartProducts", "carts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalInformation")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("uuid");

                    b.Property<string>("StripePaymentIntentId")
                        .HasColumnType("text");

                    b.Property<string>("StripeSessionId")
                        .HasColumnType("text");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("PaymentId");

                    b.ToTable("CheckoutCarts", "carts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SKU")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("StripePromotionCodeId")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Discounts", "carts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Payments", "carts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("db7346d0-b93e-402a-8025-75b393434c26"),
                            IsActive = true,
                            PaymentMethod = "card"
                        });
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ImagePathUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasPrecision(11, 2)
                        .HasColumnType("numeric(11,2)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.HasKey("Id");

                    b.ToTable("Products", "carts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Cart", b =>
                {
                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", "CheckoutCart")
                        .WithOne("Cart")
                        .HasForeignKey("Ecommerce.Modules.Carts.Core.Entities.Cart", "CheckoutCartId");

                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Discount", "Discount")
                        .WithMany("Carts")
                        .HasForeignKey("DiscountId");

                    b.Navigation("CheckoutCart");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CartProduct", b =>
                {
                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Cart", "Cart")
                        .WithMany("Products")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", "CheckoutCart")
                        .WithMany("Products")
                        .HasForeignKey("CheckoutCartId");

                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Product", "Product")
                        .WithMany("CartProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("CheckoutCart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", b =>
                {
                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Discount", "Discount")
                        .WithMany("CheckoutCarts")
                        .HasForeignKey("DiscountId");

                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Payment", "Payment")
                        .WithMany("CheckoutCarts")
                        .HasForeignKey("PaymentId");

                    b.OwnsOne("Ecommerce.Modules.Carts.Core.Entities.ValueObjects.Customer", "Customer", b1 =>
                        {
                            b1.Property<Guid>("CheckoutCartId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("CustomerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(48)
                                .HasColumnType("character varying(48)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(48)
                                .HasColumnType("character varying(48)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.HasKey("CheckoutCartId");

                            b1.ToTable("CheckoutCarts", "carts");

                            b1.WithOwner()
                                .HasForeignKey("CheckoutCartId");
                        });

                    b.OwnsOne("Ecommerce.Modules.Carts.Core.Entities.ValueObjects.Shipment", "Shipment", b1 =>
                        {
                            b1.Property<Guid>("CheckoutCartId")
                                .HasColumnType("uuid");

                            b1.Property<string>("AparmentNumber")
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("character varying(32)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<decimal>("Price")
                                .HasColumnType("numeric");

                            b1.Property<string>("Service")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("StreetName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("StreetNumber")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)");

                            b1.HasKey("CheckoutCartId");

                            b1.ToTable("CheckoutCarts", "carts");

                            b1.WithOwner()
                                .HasForeignKey("CheckoutCartId");
                        });

                    b.Navigation("Customer")
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Payment");

                    b.Navigation("Shipment");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Cart", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", b =>
                {
                    b.Navigation("Cart")
                        .IsRequired();

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Discount", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("CheckoutCarts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Payment", b =>
                {
                    b.Navigation("CheckoutCarts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Product", b =>
                {
                    b.Navigation("CartProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
