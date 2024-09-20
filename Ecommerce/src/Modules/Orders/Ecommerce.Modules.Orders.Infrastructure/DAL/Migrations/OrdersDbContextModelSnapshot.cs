﻿// <auto-generated />
using System;
using Ecommerce.Modules.Orders.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    partial class OrdersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("orders")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Complaints.Entities.Complaint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalNote")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("OrderId");

                    b.ToTable("Comaplaints", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Customers", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalInformation")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderPlacedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Payment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("Id", "OrderPlacedAt");

                    b.ToTable("Orders", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Statuses", "orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrderStatus = "Placed"
                        },
                        new
                        {
                            Id = 2,
                            OrderStatus = "ParcelPacked"
                        },
                        new
                        {
                            Id = 3,
                            OrderStatus = "Shipped"
                        },
                        new
                        {
                            Id = 4,
                            OrderStatus = "Completed"
                        },
                        new
                        {
                            Id = 5,
                            OrderStatus = "Cancelled"
                        },
                        new
                        {
                            Id = 6,
                            OrderStatus = "Returned"
                        });
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Returns.Entity.Return", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalNote")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("ReasonForReturn")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.HasIndex("OrderId");

                    b.ToTable("Returns", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Complaints.Entities.Complaint", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", "Customer")
                        .WithOne("Complaint")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Complaints.Entities.Complaint", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Complaints.Entities.ComplaintProduct", "Products", b1 =>
                        {
                            b1.Property<Guid>("ComplaintId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ImagePathUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(24)
                                .HasColumnType("character varying(24)");

                            b1.Property<decimal>("Price")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.Property<int?>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("SKU")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.HasKey("ComplaintId", "Id");

                            b1.ToTable("ComplaintProducts", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ComplaintId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Order");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", "Customer")
                        .WithOne("Order")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Orders.Entities.Product", "Products", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ImagePathUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(24)
                                .HasColumnType("character varying(24)");

                            b1.Property<decimal>("Price")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.Property<int?>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("SKU")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("Products", "orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Shipment", "Shipment", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("ApartmentNumber")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(32)
                                .HasColumnType("character varying(32)");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<string>("StreetName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<string>("StreetNumber")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders", "orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Products");

                    b.Navigation("Shipment")
                        .IsRequired();
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Returns.Entity.Return", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", "Customer")
                        .WithOne("Return")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Returns.Entity.Return", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Returns.Entity.ReturnProduct", "Products", b1 =>
                        {
                            b1.Property<Guid>("ReturnId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ImagePathUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(24)
                                .HasColumnType("character varying(24)");

                            b1.Property<decimal>("Price")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.Property<int?>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("SKU")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.HasKey("ReturnId", "Id");

                            b1.ToTable("ReturnProducts", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ReturnId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Order");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", b =>
                {
                    b.Navigation("Complaint");

                    b.Navigation("Order")
                        .IsRequired();

                    b.Navigation("Return");
                });
#pragma warning restore 612, 618
        }
    }
}
