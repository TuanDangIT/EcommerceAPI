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
                .HasAnnotation("ProductVersion", "8.0.11")
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

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("Id", "CreatedAt");

                    b.ToTable("Complaints", "orders", t =>
                        {
                            t.HasCheckConstraint("CK_Complaint_RefundAmount", "\"Decision_RefundAmount\" >= 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Customers", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InvoiceNo")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Invoices", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientAdditionalInformation")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CompanyAdditionalInformation")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Payment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StripePaymentIntentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id", "CreatedAt");

                    b.ToTable("Orders", "orders", t =>
                        {
                            t.HasCheckConstraint("CK_Order_DiscountValue", "\"Discount_Value\" > 0");

                            t.HasCheckConstraint("CK_Order_TotalSum", "\"TotalSum\" >= 0");
                        });
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("LabelCreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LabelId")
                        .HasColumnType("text");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Service")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("text");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Shipments", "orders");
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

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Returns.Entities.Return", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalNote")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsFullReturn")
                        .HasColumnType("boolean");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("ReasonForReturn")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("RejectReason")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("Id", "CreatedAt");

                    b.ToTable("Returns", "orders");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Complaints.Entities.Complaint", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithMany("Complaints")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Complaints.Entities.Decision", "Decision", b1 =>
                        {
                            b1.Property<Guid>("ComplaintId")
                                .HasColumnType("uuid");

                            b1.Property<string>("AdditionalInformation")
                                .HasColumnType("text");

                            b1.Property<string>("DecisionText")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<decimal?>("RefundAmount")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.HasKey("ComplaintId");

                            b1.ToTable("Complaints", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ComplaintId");
                        });

                    b.Navigation("Decision");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithOne("Customer")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Orders.Entities.Customer", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("BuildingNumber")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("CountryCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("PostCode")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customers", "orders");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Invoice", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithOne("Invoice")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Orders.Entities.Invoice", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", b =>
                {
                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Orders.Entities.Product", "Products", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ImagePathUrl")
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<decimal>("Price")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("SKU")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("numeric");

                            b1.HasKey("OrderId", "Id");

                            b1.ToTable("Products", "orders", t =>
                                {
                                    t.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0");

                                    t.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");

                                    t.HasCheckConstraint("CK_Product_UnitPrice", "\"UnitPrice\" >= 0");
                                });

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.DeliveryService", "DeliveryService", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Courier")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<decimal>("Price")
                                .HasColumnType("numeric");

                            b1.Property<string>("Service")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders", "orders", t =>
                                {
                                    t.HasCheckConstraint("CK_Order_DeliveryService_Price", "\"DeliveryService_Price\" >= 0");
                                });

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Discount", "Discount", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasMaxLength(48)
                                .HasColumnType("character varying(48)");

                            b1.Property<string>("SKU")
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<decimal>("Value")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders", "orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("DeliveryService")
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Shipment", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithMany("Shipments")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Insurance", "Insurance", b1 =>
                        {
                            b1.Property<int>("ShipmentId")
                                .HasColumnType("integer");

                            b1.Property<string>("Amount")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ShipmentId");

                            b1.ToTable("Shipments", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");
                        });

                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Parcel", "Parcels", b1 =>
                        {
                            b1.Property<int>("ShipmentId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.HasKey("ShipmentId", "Id");

                            b1.ToTable("Parcels", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");

                            b1.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Dimensions", "Dimensions", b2 =>
                                {
                                    b2.Property<int>("ParcelShipmentId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("ParcelId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Height")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Length")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Unit")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Width")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("ParcelShipmentId", "ParcelId");

                                    b2.ToTable("Parcels", "orders");

                                    b2.WithOwner()
                                        .HasForeignKey("ParcelShipmentId", "ParcelId");
                                });

                            b1.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Weight", "Weight", b2 =>
                                {
                                    b2.Property<int>("ParcelShipmentId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("ParcelId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Amount")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Unit")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("ParcelShipmentId", "ParcelId");

                                    b2.ToTable("Parcels", "orders");

                                    b2.WithOwner()
                                        .HasForeignKey("ParcelShipmentId", "ParcelId");
                                });

                            b1.Navigation("Dimensions")
                                .IsRequired();

                            b1.Navigation("Weight")
                                .IsRequired();
                        });

                    b.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Receiver", "Receiver", b1 =>
                        {
                            b1.Property<int>("ShipmentId")
                                .HasColumnType("integer");

                            b1.Property<string>("CompanyName")
                                .HasColumnType("text");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ShipmentId");

                            b1.ToTable("Shipments", "orders");

                            b1.WithOwner()
                                .HasForeignKey("ShipmentId");

                            b1.OwnsOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects.Address", "Address", b2 =>
                                {
                                    b2.Property<int>("ReceiverShipmentId")
                                        .HasColumnType("integer");

                                    b2.Property<string>("BuildingNumber")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Country")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("CountryCode")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("PostCode")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("ReceiverShipmentId");

                                    b2.ToTable("Shipments", "orders");

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiverShipmentId");
                                });

                            b1.Navigation("Address")
                                .IsRequired();
                        });

                    b.Navigation("Insurance");

                    b.Navigation("Order");

                    b.Navigation("Parcels");

                    b.Navigation("Receiver")
                        .IsRequired();
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Returns.Entities.Return", b =>
                {
                    b.HasOne("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", "Order")
                        .WithOne("Return")
                        .HasForeignKey("Ecommerce.Modules.Orders.Domain.Returns.Entities.Return", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Ecommerce.Modules.Orders.Domain.Returns.Entities.ReturnProduct", "Products", b1 =>
                        {
                            b1.Property<Guid>("ReturnId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("ImagePathUrl")
                                .HasColumnType("text");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("character varying(64)");

                            b1.Property<decimal>("Price")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.Property<string>("SKU")
                                .IsRequired()
                                .HasMaxLength(16)
                                .HasColumnType("character varying(16)");

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<decimal>("UnitPrice")
                                .HasPrecision(11, 2)
                                .HasColumnType("numeric(11,2)");

                            b1.HasKey("ReturnId", "Id");

                            b1.ToTable("ReturnProducts", "orders", t =>
                                {
                                    t.HasCheckConstraint("CK_ReturnProduct_Price", "\"Price\" >= 0");

                                    t.HasCheckConstraint("CK_ReturnProduct_Quantity", "\"Quantity\" >= 0");

                                    t.HasCheckConstraint("CK_ReturnProduct_UnitPrice", "\"UnitPrice\" >= 0");
                                });

                            b1.WithOwner()
                                .HasForeignKey("ReturnId");
                        });

                    b.Navigation("Order");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Ecommerce.Modules.Orders.Domain.Orders.Entities.Order", b =>
                {
                    b.Navigation("Complaints");

                    b.Navigation("Customer");

                    b.Navigation("Invoice");

                    b.Navigation("Return");

                    b.Navigation("Shipments");
                });
#pragma warning restore 612, 618
        }
    }
}
