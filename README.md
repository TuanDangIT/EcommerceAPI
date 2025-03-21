# EcommerceAPI

![EcommerceAPI logo](./assets/Logo.png)

## Overview

The Headless CMS Ecommerce API facilitates the process of selling goods and services online, from checkout to post-purchase support. It handles order processing, invoice generation, and shipping label printing, ensuring smooth fulfillment. The API also manages returns, complaints, and other after-sales processes to enhance customer experience. As a headless solution, it integrates seamlessly with any frontend, offering flexibility for businesses of all sizes.

# Table of Contents

- [EcommerceAPI](#ecommerceapi)
  - [Overview](#overview)
- [Table of Contents](#table-of-contents)
- [Design](#design)
  - [Event storming](#event-storming)
    - [Bounded contexts](#bounded-contexts)
  - [Domain models](#domain-models)
    - [Order](#order)
    - [Product](#product)
    - [Auction](#auction)
    - [Cart](#cart)
  - [Architecture](#architecture)
  - [File structure](#file-structure)
- [Getting started](#getting-started)
- [Documentation](#documentation)
  - [Authentication and Authorization](#authentication-and-authorization)
  - [Carts module](#carts-module)
    - [Carts](#carts)
    - [Checkout Carts](#checkout-carts)
    - [Payments](#payments)
  - [Discounts module](#discounts-module)
    - [Coupons](#coupons)
    - [Discounts](#discounts)
    - [Offers](#offers)
  - [Inventory module](#inventory-module)
    - [Auctions](#auctions)
    - [Categories](#categories)
    - [Manufacturers](#manufacturers)
    - [Parameters](#parameters)
    - [Products](#products)
    - [Reviews](#reviews)
  - [Orders module](#orders-module)
    - [Orders](#orders)
    - [Invoices](#invoices)
    - [Shipments](#shipments)
    - [Returns](#returns)
    - [Complaints](#complaints)
  - [Users module](#users-module)
    - [Users](#users)
    - [Employees](#employees)
    - [Customers](#customers)
    - [Tokens](#tokens)
    - [Roles](#roles)
  - [Mails module](#mails-module)
- [Work to be done](#work-to-be-done)
- [Technology](#technology)
- [License](#license)

# Design

The design section will provide an overview of the application's architecture, focusing on the use of event storming to map out business processes and identify key events within the system.

## Event storming

![Event storming using sticky notes and displayed on the timeline](/assets/event-storming-timeline.png)

### Bounded contexts

![Event storming grouped to become modules](/assets/event-storming-bounded-contexts.png)

## Domain models

Only most important domain models C# code will be displayed below.

### Order

```cs
public class Order : AggregateRoot, IAuditable
{
    public Customer Customer { get; private set; } = default!;
    private readonly List<Product> _products = [];
    public IEnumerable<Product> Products => _products;
    public decimal TotalSum { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Placed;
    public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
    public string? ClientAdditionalInformation { get; private set; }
    public string? CompanyAdditionalInformation { get; private set; }
    public Discount? Discount { get; private set; }
    public string StripePaymentIntentId { get; private set; } = string.Empty;
    public string ShippingService { get; private set; } = string.Empty;
    public decimal ShippingPrice { get; private set; }
    private readonly List<Shipment> _shipments = [];
    public IEnumerable<Shipment> Shipments => _shipments;
    public Return? Return { get; private set; }
    private readonly List<Complaint>? _complaints = [];
    public IEnumerable<Complaint>? Complaints => _complaints;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Invoice? Invoice { get; private set; }
    public bool HasInvoice => Invoice is not null;
}
```

### Product

```cs
public class Product : AggregateRoot, IAuditable
{
    public new Guid Id { get; private set; }
    public string SKU { get; private set; } = string.Empty;
    public string? EAN { get; private set; }
    public string Name { get; private set; } = string.Empty;
    //Currency will be added later
    //public string Currency { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int VAT { get; private set; }
    public int? Quantity { get; private set; }
    public bool HasQuantity => Quantity != null;
    public bool IsSold => Quantity == 0;
    public int? Reserved { get; private set; }
    public string? Location { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? AdditionalDescription { get; private set; }
    public bool IsListed { get; private set; } = false;
    private readonly List<Parameter> _parameters = [];
    public IEnumerable<Parameter> Parameters => _parameters;
    private List<ProductParameter> _productParameters = [];
    public IEnumerable<ProductParameter> ProductParameters => _productParameters;
    public Manufacturer? Manufacturer { get; private set; } 
    public Guid? ManufacturerId { get; private set; }
    public List<Image> _images = [];
    public IEnumerable<Image> Images => _images;
    public Category? Category { get; private set; }
    public Guid? CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
}
```

### Auction

```cs
public class Auction : AggregateRoot, IAuditable
{
    public string SKU { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int? Quantity { get; private set; }
    public bool IsSold { get; set; } = false;
    public bool HasQuantity => Quantity != null;
    public string Description { get; private set; } = string.Empty;
    public string? AdditionalDescription { get; private set; }
    public List<AuctionParameter>? Parameters { get; private set; } 
    public string? Manufacturer { get; private set; }
    public List<string> ImagePathUrls { get; private set; } = [];
    public string? Category { get; private set; } = string.Empty;
    private readonly List<Review> _review = [];
    public IEnumerable<Review> Reviews => _review;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
}
```

### Cart

```cs
    public class Cart : BaseEntity, IAuditable
    {
        private readonly List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public decimal TotalSum { get; private set; }
        public Discount? Discount { get; private set; }
        public int? DiscountId { get; private set; }
        public CheckoutCart? CheckoutCart { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool HasDiscount => Discount is not null;
    }
```


## Architecture

Everything will wrapped as a modular monolith.

![Architecture](/assets/architecture.png)

## File structure

![Solution file structure of the API](/assets/file-structure.png)

# Getting started

# Documentation

The documentation section will include detailed information about each module, outlining the functionality and associated endpoints for each. It will cover the various API modules, such as order management, payment processing, product listings, and customer interactions, with clear explanations of the available endpoints, request/response formats, and usage examples.

## Authentication and Authorization

The Ecommerce API utilizes JWT (JSON Web Tokens) for secure and efficient authentication, ensuring that only authorized users can access the system. For authorization, the API implements Role-Based Access Control (RBAC), which assigns specific permissions and access levels based on the user's role within the system.

## Carts module

### Carts

![Carts endpoints](/assets/carts-swagger.png)

### Checkout Carts

![Checkout carts endpoints](/assets/checkoutcarts-swagger.png)

### Payments

![Payments endpoints](/assets/payments-swagger.png)

## Discounts module

### Coupons

![Coupons endpoints](/assets/coupons-swagger.png)

### Discounts

![Discount's endpoints](/assets/discounts-swagger.png)

### Offers

![Offers endpoints](/assets/offers-swagger.png)

## Inventory module

### Auctions

![Auctions endpoints](/assets/auctions-swagger.png)

### Categories

![Categorys endpoints](/assets/categories-swagger.png)

### Manufacturers

![Manufacturers endpoints](/assets/manufacturers-swagger.png)

### Parameters

![Parameters endpoints](/assets/parameters-swagger.png)

### Products

![Products endpoints](/assets/products-swagger.png)

### Reviews

![Reviews endpoints](/assets/reviews-swagger.png)

## Orders module

### Orders

![Orders endpoints](/assets/orders-swagger.png)

### Invoices

![Invoices endpoints](/assets/invoices-swagger.png)

### Shipments

![Shipments endpoints](/assets/shipments-swagger.png)

### Returns

![Returns endpoints](/assets/returns-swagger.png)

### Complaints

![Complaints endpoints](/assets/complaints-swagger.png)

## Users module

### Users

![Users endpoints](/assets/users-swagger.png)

### Employees

![Employees endpoints](/assets/employees-swagger.png)

### Customers

![Customers endpoints](/assets/customers-swagger.png)

### Tokens

![Tokens endpoints](/assets/tokens-swagger.png)

### Roles

![Roles endpoints](/assets/roles-swagger.png)

## Mails module

![Mails endpoints](/assets/mails-swagger.png)

# Work to be done

API Improvement Task List:
- [ ] Add functionality for adding and deleting parameters, but only for this purpose.  
- [ ] Create a job to deactivate expired discounts once per day.  
- [ ] Add shipment functionality and integrate other shipping providers besides InPost.  
- [ ] Implement tax handling, but likely before shipment processing.  
- [ ] Add more payment options.  
- [ ] Send a Stripe checkout link to users for manual/phone orders.  
- [ ] Improve module communication by adding synchronous API calls instead of only integration events.  
- [ ] Implement order archiving as a background task.  
- [ ] Add refund history and allow refund cancellations.  
- [ ] Complete work on draft orders.  
- [ ] Implement generic cursor pagination.  
- [ ] Make DTOs immutable using records.  
- [ ] Allow multiple returns per order. Currently, only one return per order is allowed.  
- [ ] Implement return and complaint functionality where refunds must be processed by the end of the day or within 24 hours, allowing time for cancellation. Likely requires Hangfire.  
- [ ] Highlight discounted products in the order summary.  
- [ ] Evaluate whether using SKUs as product identifiers in orders for returns is a correct approach.  
- [ ] Add more debug logging.  
- [ ] Allow users to modify settings via `appsettings` or API.  
- [ ] Add logging for `Order` and `Return` entities from the user's perspective.  
- [ ] Prevent adding the same product multiple times to a return by linking it to its SKU. If the product status is already "returned," it cannot be added again.  

# Technology

List of technologies, frameworks and libraries used for implementation:

- .NET 8
- C#
- PostgreSQL
- Entity Framework Core
- Swashbuckle
- FluentValidation
- MediatR
- Serilog
- Seq
- Azure Blob Storage
- Azurite
- Scrutor
- Sieve
- Stripe.net
- Polly
- Coravel
- CsvHelper
- MailKit
- Select.HtmlToPdf
- JWT
- xUnit
- Docker

# License

The project is under [MIT license](https://opensource.org/license/MIT)
