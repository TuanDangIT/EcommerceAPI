# EcommerceAPI

![EcommerceAPI logo](./assets/Logo.png)

## Overview

The Headless CMS Ecommerce API facilitates the process of selling goods and services online, from checkout to post-purchase support. It handles order processing, invoice generation, and shipping label printing, ensuring smooth fulfillment. The API also manages returns, complaints, and other after-sales processes to enhance customer experience. As a headless solution, it integrates seamlessly with any frontend, offering flexibility for businesses of all sizes.

### Disclaimer before proceeding with the documentation and code

The EcommerceAPI project was created to demonstrate my skills as a future .NET Developer. It is not intended for commercial use in its current state. This project includes various shortcuts and simplifications for demonstration purposes, making it unsuitable for real-world implementations.

Many aspects of the application will be improved or reworked in future versions of the API. I acknowledge that there may be mistakes or areas for improvement—some of which I may have overlooked, while others were intentionally left due to time limitation.

If you come across any issues, suggestions, or potential improvements, I would greatly appreciate your feedback. Please feel free to reach out to me at shocksee001@gmail.com.

Thank you for your understanding and interest!

# Table of Contents

- [EcommerceAPI](#ecommerceapi)
  - [Overview](#overview)
    - [Disclaimer before proceeding with the documentation and code](#disclaimer-before-proceeding-with-the-documentation-and-code)
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
  - [Payment processing](#payment-processing)
  - [Delivery system](#delivery-system)
- [Getting started](#getting-started)
  - [appsettings.json](#appsettingsjson)
  - [Docker](#docker)
  - [Csv import file](#csv-import-file)
- [Documentation](#documentation)
  - [Authentication and Authorization](#authentication-and-authorization)
  - [Pagination](#pagination)
    - [Offset pagination](#offset-pagination)
    - [Cursor pagination](#cursor-pagination)
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

Only most important domain models C# code will be displayed below (just properties withour any domain methods).

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

## Payment processing

The system uses Stripe for payment processing. However, it currently operates in test mode rather than the official production version. Stripe is a financial technology company that enables businesses to process payments, manage subscriptions, and handle financial transactions securely.

After invoking place order a checkout link will be given to the user in a HTTP response. To complete the transaction successfully, use the test card number **4242 4242 4242 4242**. Other card details can be customized freely.

![Stripe success card details](/assets/stripe-card-details.png)

## Delivery system

For deliveries, the system integrates with InPost, a Polish logistics company specializing in automated parcel lockers and courier services. Currently, InPost is the only supported courier in the API. Future updates will introduce support for international providers such as DHL, DPD, and GLS.

# Getting started

This section provides essential steps to quickly set up and run the application.

## appsettings.json

Before getting started, you need to configure essential details for external services such as delivery and payment systems to ensure the API functions correctly. This is managed through a centralized configuration called appsettings.

The configuration is divided into two files:

- One for general settings, which serve as a base for all environments.

- One specifically for Development settings.

In the future, an additional file will be added for Production.

Besides key details, the configuration also includes settings for pagination, authentication, and other adjustable parameters.

Most important details to fill are:

- Stripe\_\_ApiKey
- Stripe \_\_ WebhookSecret (this can be found on stripe dashboard or in stripe CLI in docker: [Docker webhook secret in stripe CLI](/assets/docker-webhook-secret.png))
- InPost\_\_OrganizationId
- InPost\_\_ApiKey
- Mail\_\_Email
- Mail\_\_Password
- Mail \_\_SmtpHost and \_\_SmtpPort (default provider is Gmail, but this can be changed)
- Company details

Access the configuration files here: [appsettings.json](/Ecommerce/src/Bootstrapper/Ecommerce.Bootstrapper/appsettings.json) and [appsettings.Development.json](/Ecommerce/src/Bootstrapper/Ecommerce.Bootstrapper/appsettings.Development.json).

_Note: All these details can also be changed via docker-compose.yml file where you can manage them as environment variables_

## Docker

After configuring the necessary information in the appsettings.json file, you have two options:

- Manual Setup – Install an IDE like Visual Studio along with external dependencies such as PostgreSQL and Seq on your local machine.

- [Docker](https://www.docker.com) – Use Docker to run all dependencies in a containerized environment, eliminating the need for manual installation and keeping your system clean.

With Docker, everything runs in an isolated environment, ensuring consistency across different setups.

The application requires the following external services:

- PostgreSQL
- Seq
- Stripe CLI
- Azurite Emulator

To start the infrastructure using [Docker](https://www.docker.com), run:

```
docker compose up -d
```

After executing the command, all required containers should be up and running:
![Docker desktop containers](/assets/docker.png)

## Csv import file

In the assets, there is a CSV import file named [ProductImportFile.csv](/assets/ProductImportFile.csv), which contains sample products that can be added to the database. This helps streamline the user testing experience, allowing for a quick start and easy API testing.
The file includes only a few products by default, but you can add more entries directly, which is faster than manual insertion via API endpoints.
Please keep in mind the constraints on certain properties, such as EAN, which must be exactly 13 characters long. Otherwise, a database error will be thrown. _While these constraints may not yet be enforced, they will be implemented soon in future commits_.

# Documentation

The documentation section will include detailed information about each module, outlining the functionality and associated endpoints for each. It will cover the various API modules, such as order management, payment processing, product listings, and customer interactions, with clear explanations of the available endpoints, request/response formats, and usage examples.

The description is included within every endpoint. Apart from swagger descriptions there will be text for endpoints that require further explanation, because of the more complicated business logic and therefor implementation.

The description is included in every endpoint in the swagger images. For most it's just CRUD operations. In addition to Swagger descriptions, additional text will be provided for endpoints requiring further explanation due to their complex business logic and implementation.

## Authentication and Authorization

The Ecommerce API utilizes JWT (JSON Web Tokens) for secure and efficient authentication, ensuring that only authorized users can access the system. For authorization, the API implements Role-Based Access Control (RBAC), which assigns specific permissions and access levels based on the user's role within the system.

## Pagination

The EcommerceAPI implements two types of pagination algorithms: offset pagination and cursor pagination. Entities are ordered based on the cluster index of their ID property and CreatedAt datetime.

For date filtering, you can use either date strings or dates with time, but all values must be in UTC format.

### Offset pagination

Offset pagination is used for simpler cases where performance is not significantly affected by large datasets, such as the Product entity. To simplify the implementation, the API leverages Sieve, an external library that reduces the need for additional pagination logic. Sieve is a mature and versatile library that enables filtering and sorting for entities that support browsing.

Each entity using Sieve pagination has a dedicated configuration specifying which properties can be filtered or sorted and how.

Example of a sieve configuration for coupons:

```cs
 internal class CouponSieveConfiguration : ISieveConfiguration
 {
     public void Configure(SievePropertyMapper mapper)
     {
         mapper.Property<Coupon>(c => c.Id)
             .CanFilter();
         mapper.Property<Coupon>(c => c.CreatedAt)
             .CanFilter()
             .CanSort();
         mapper.Property<Coupon>(c => c.Type)
             .CanFilter();
         mapper.Property<Coupon>(c => c.Name)
             .CanFilter()
             .CanSort();
         mapper.Property<Coupon>(c => c.Redemptions)
            .CanSort();
         mapper.Property<NominalCoupon>(nc => nc.NominalValue)
             .CanFilter()
             .CanSort();
         mapper.Property<PercentageCoupon>(pc => pc.Percent)
             .CanFilter()
             .CanSort();
     }
 }
```

Below is a table of available filter operators and their meanings:
| Operator | Meaning |
|------------|--------------------------|
| `==` | Equals |
| `!=` | Not equals |
| `>` | Greater than |
| `<` | Less than |
| `>=` | Greater than or equal to |
| `<=` | Less than or equal to |
| `@=` | Contains |
| `_=` | Starts with |
| `_-=` | Ends with |
| `!@=` | Does not Contains |
| `!_=` | Does not Starts with |
| `!_-=` | Does not Ends with |
| `@=*` | Case-insensitive string Contains |
| `_=*` | Case-insensitive string Starts with |
| `_-=*` | Case-insensitive string Ends with |
| `==*` | Case-insensitive string Equals |
| `!=*` | Case-insensitive string Not equals |
| `!@=*` | Case-insensitive string does not Contains |
| `!_=*` | Case-insensitive string does not Starts with |

For properties in many-to-many relationships (e.g., parameters and products), custom filter methods are required. To use these filters, you must specify the method name as the property name in the filter argument. It’s important to note that custom Sieve filters only support equality (==) and containment (@=) operators.

Example of a custom filter method for products and their parameters:

```cs
internal class SieveCustomFilterMethods : ISieveCustomFilterMethods
{
    public IQueryable<Product> ProductParameterNameAndValue(IQueryable<Product> source, string op, string[] values)
    {
        //This filter is for first when values length is equal to 0 then it filters only by name, if 2 then by name and value associated with that parameter name.
        switch ((op, values.Length))
        {
            case ("==", 1):
                return source.Where(p => p.Parameters.Any(p => values.Any(v => v.ToLower() == p.Name.ToLower())));
            case ("==", 2):
                return source.Where(p => p.Parameters.Any(pa => values[0].ToLower() == pa.Name.ToLower() && p.ProductParameters.First(pp => pp.ParameterId == pa.Id).Value.ToLower() == values[1].ToLower()));
            case ("@=", 1):
                return source.Where(p => p.Parameters.Any(p => values.Any(v => p.Name.ToLower().Contains(v.ToLower()))));
            case ("@=", 2):
                return source.Where(p => p.Parameters.Any(pa => pa.Name.ToLower().Contains(values[0].ToLower()) && p.ProductParameters.First(pp => pp.ParameterId == pa.Id).Value.ToLower().Contains(values[1].ToLower())));
            default:
                break;
        }
        return source;
    }
}
```

Example of filter usage in query:

```
Filter=Name=Adam - which means search for user with a name "Adam"

Filter=Coupon.Name==50OFF - which means that you are filtering through coupons and then looking for exact name "50OFF"

Filter=ProductParameterNameAndValue==Size|30 - which means that you are looking for product that has size with value 30
```

For more information about Sieve. Please click at the [link](https://github.com/Biarity/Sieve).

### Cursor pagination

Unlike offset pagination, cursor pagination does not have a widely available external library. Therefore, a custom implementation was developed, inspired by [Julio Casal’s approach](https://juliocasal.com/blog/ASP.NET-Core-Pagination-For-Large-Datasets) and modified to also include the CreatedAt property in the pagination algorithm. Additionally, support has been added for various C# types such as Object, Enums and Collections. For objects, filtering is currently limited to two levels deep from the main entity. For example, you can filter on Order.Shipments.TrackingNumber, but not beyond that level. Cursor pagination is primarily used for entities that can rapidly grow in number, such as the Order entity, which can accumulate millions of records in a short time. The trade-off, however, is that users cannot jump to specific pages—only forward and backward navigation is supported. Another limitation is that, unlike offset pagination with Sieve, which supports multiple filter operators, this cursor implementation, because it is self-developed, only supports equality (==) and containment (@=) checks.

To see the list of available filterable properties, check the validator classes. While currently included in the validators for simplicity, this logic will soon be moved to dedicated classes for improved readability and maintainability.

Example of an validator with available/supported filters:

```cs
public class BrowseOrderValidator : AbstractValidator<BrowseOrders>
{
    private readonly string[] _availableFilters = ["Id", "TotalSum", "CreatedAt", "Discount.Code", "Discount.Type", "Payment", "Status",
        "Customer.UserId", "Customer.FirstName", "Customer.LastName", "Customer.Email", "Customer.FirstName",
        "Shipments.TrackingNumber", "Shipments.Service", "Shipments.Id", "Shipments.LabelCreatedAt", "Products.SKU", "Products.Name"];
    public BrowseOrderValidator()
    {
        RuleForEach(b => b.Filters)
            .Custom((keyValuePair, context) =>
            {
                if (!_availableFilters.Select(a => a.ToLower()).Contains(keyValuePair.Key.ToLower()))
                {
                    context.AddFailure($"Provided filter is not supported. Please use the following ones: {string.Join(", ", _availableFilters)}.");
                }
            });
    }
}
```

Example of cursor filter usage in body:

```
{
  "TotalSum": "500",
  "Shipments.TrackingNumber": "000000000000"
}
```

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

Below is a list of tasks to enhance the quality of the API. They will all be done one by one in the future commits.

API Improvement Task List:

- [ ] Add functionality for adding, deleting parameters and updating their value for a product,
- [ ] Create a job to deactivate expired discounts once per day.
- [ ] Integrate other shipping providers besides InPost.
- [ ] Implement tax handling.
- [ ] Add more payment options.
- [ ] Send a Stripe checkout link to users for manual/phone orders.
- [ ] Improve module communication by adding synchronous API calls instead of only integration events. This will help deleting unnecassary properties from database and getting the dynamically through public API.
- [ ] Implement order archiving as a background task.
- [ ] Add history logs for orders ex. refunds, returns etc..
- [ ] Add cancel feature for refunds via Stripe
- [ ] Able to create drafts (empty orders) on company side for phone/personal orders.
- [ ] Implement generic cursor pagination, if possible.
- [ ] Make DTOs immutable using records.
- [ ] Allow multiple returns per order. Currently, only one return per order is allowed.
- [ ] Implement return and complaint functionality where refunds must be processed by the end of the day or within 24 hours, allowing time for cancellation. Likely requires Hangfire.
- [ ] Highlight discounted products in the order summary.
- [ ] Evaluate whether using SKUs as product identifiers in orders for returns is a correct approach.
- [ ] Add more debug logging ex. handling integration events.
- [ ] Allow users/company to modify settings such as currency and other editible areas via `appsettings` or API.
- [ ] Enhance authorization for reviews, orders, returns when submitting return (property customerID must be equal to context CustomerId) etc. to able to modify only if the user ID matches CustomerId property in the entity.
- [ ] Add Github Actions CI/CD after finishing unit and integration tests.
- [ ] Allow CSV exports for invoices and orders.
- [ ] Enable invoice editting.
- [ ] Validate CSV product imports.
- [ ] Autocomplete of customer's details when logged in.
- [ ] Make discount Id as type Guid to facilite integration with carts module.
- [ ] Put submit return and complaint in it's respective controllers, instead of having it in OrdersController.
- [ ] Make API more resilient to errors by implementing rollbacks for extenal services such as Stripe, InPost or Azurite.

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
- Azurite Emulator
- Scrutor
- Sieve
- Stripe/Stripe .NET (test environemt)
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
