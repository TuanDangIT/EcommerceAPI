﻿// <auto-generated />
using System;
using Ecommerce.Modules.Carts.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Carts.Core.DAL.Migrations
{
    [DbContext(typeof(CartsDbContext))]
    [Migration("20240901195309_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Carts");
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

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("CheckoutCartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartProducts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.ToTable("CheckoutCarts");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Payments");
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
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Products");
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
                    b.HasOne("Ecommerce.Modules.Carts.Core.Entities.Payment", "Payment")
                        .WithMany("CheckoutCarts")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Ecommerce.Modules.Carts.Core.Entities.Shipment", "Shipment", b1 =>
                        {
                            b1.Property<Guid>("CheckoutCartId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("character varying(32)");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<string>("ReceiverFullName")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("character varying(32)");

                            b1.Property<string>("StreetName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<int>("StreetNumber")
                                .HasMaxLength(8)
                                .HasColumnType("integer");

                            b1.HasKey("CheckoutCartId");

                            b1.ToTable("CheckoutCarts");

                            b1.WithOwner()
                                .HasForeignKey("CheckoutCartId");
                        });

                    b.Navigation("Payment");

                    b.Navigation("Shipment")
                        .IsRequired();
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.Cart", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Carts.Core.Entities.CheckoutCart", b =>
                {
                    b.Navigation("Products");
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
