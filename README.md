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
- [Tests](#tests)
  - [Unit tests](#unit-tests)
  - [Integration tests](#integration-tests)
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

The Carts controller enables users to perform CRUD operations on their carts. Additionally, users can add or remove products as needed. A discount system is implemented to enhance the shopping experience. Once users have selected their items, they can proceed to checkout. Notably, even after a Checkout cart is created, all cart endpoints remain accessible, ensuring any changes made are instantly synchronized with the corresponding Checkout cart.

![Carts endpoints](/assets/carts-swagger.png)

### Checkout Carts

This controller allows users to view Checkout cart details and configure essential order processing information, including customer details, payment method, and shipment details. Once finalized, users can place an order, receiving a Stripe payment link to complete the transaction. Upon successful payment, Stripe triggers a webhook endpoint, leading to order creation and automatic product quantity reduction. After successfull purchase, an email will be sent to the customer's email.

![Checkout carts endpoints](/assets/checkoutcarts-swagger.png)

### Payments

The Payment controller enables users to view available payment methods, while allowing companies to configure their payment policy by enabling or disabling specific payment options.

![Payments endpoints](/assets/payments-swagger.png)

## Discounts module

### Coupons

These endpoints allow a company to create, update, and delete coupons. Coupons are Stripe-related entities used to apply discounts. A company can create either a nominal discount, which subtracts a fixed amount from the cart total, or a percentage discount, which reduces the total by a specified percentage.

![Coupons endpoints](/assets/coupons-swagger.png)

### Discounts

The Discount controller enables CRUD operations. To access a discount, a coupon must first be specified in the route. Discounts are Stripe-related entities that include a code entered by the user in the discount field and an expiration date.

![Discount's endpoints](/assets/discounts-swagger.png)

### Offers

The Offer entity is generated through the auction request endpoint. Once created, an employee can choose to accept or reject it. Upon acceptance, a discount with a unique code is created. Notably, this discount is independent of any coupon or Stripe service.

![Offers endpoints](/assets/offers-swagger.png)

## Inventory module

### Auctions

The auctions controller is very simple. It only allows browsing, getting specified auction by id and sending offer request.

![Auctions endpoints](/assets/auctions-swagger.png)

### Categories

![Categorys endpoints](/assets/categories-swagger.png)

### Manufacturers

![Manufacturers endpoints](/assets/manufacturers-swagger.png)

### Parameters

![Parameters endpoints](/assets/parameters-swagger.png)

### Products

This is one of the key controllers in the API, supporting standard CRUD operations. Additionally, it provides quick updates for individual properties such as price or quantity. There are also two endpoints for managing product visibility for end customers: listing and unlisting. Notably, a product must be unlisted before it can be updated.

![Products endpoints](/assets/products-swagger.png)

### Reviews

This controller handles the review of active auctions. Auctions that are later delisted will remain linked, even if they are relisted.

![Reviews endpoints](/assets/reviews-swagger.png)

## Orders module

### Orders

The Orders controller allows a company to manage order processing, invoice generation, and shipping label printing. It also handles returns, complaints, and other after-sales processes to improve the customer experience. An endpoint for order cancellation is available, but it must occur within 30 minutes of the order being placed. Currently, the time limit is set directly in the class, though it will soon be configurable through the appsettings.json file. After this period, customers can only return products through the return process. The API also supports submitting complaints, with each order able to have multiple complaints associated with it. At the end, there are two webhooks for order status updates, triggered by the delivery system. After invoking some endpoints such as cancelling an order or returning it, an email will sent via mailkit to the end customer.

![Orders endpoints](/assets/orders-swagger.png)

### Invoices

![Invoices endpoints](/assets/invoices-swagger.png)

### Shipments

This controller allows a company to create shipments by integrating with external services. Currently, the only supported provider is InPost, though more providers will be added in the future. An employee can generate a shipping label, which can then be downloaded as a PDF after a few seconds. While an order can have multiple shipments, the shipping cost is applied only once.

![Shipments endpoints](/assets/shipments-swagger.png)

### Returns

After invoking the return endpoint in the Orders controller, a return entity is created and managed in the Return controller. Here, the return can either be handled or rejected. When a return is handled, the Stripe service is triggered to refund the customer. It is worth to mention that each operation on return including handling it and rejecting invokes a mail sending notification to the customer. The controller also allows for product manipulation, where employees can add or remove products from the return. All changes are synchronized with the order entity—removing a product from the return will also remove it from the order, and vice versa.

![Returns endpoints](/assets/returns-swagger.png)

### Complaints

The Complaints controller manages the acceptance and rejection of complaints. If accepted, a partial or full refund will be processed via the Stripe service. These actions will also trigger an email notification to the customer.

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

The primary role of the Mails module is to send notifications to clients about the status of their orders, offers, returns, and complaints. In addition, the module also provides endpoints for sending emails to both customers and non-customers (email address is needed). It supports two types of browsing: paginated browsing, which allows filtering by Order ID or Customer ID, and offset-based browsing, which is intended for handling larger volumes of data, such as viewing all sent emails. The module also includes functionality for downloading files that were attached to the emails.

![Mails endpoints](/assets/mails-swagger.png)

# Tests

## Unit tests

The solution includes a Tests folder containing unit tests for each module. These tests are written using the xUnit framework, along with popular libraries such as FluentAssertions and Moq.

Example of unit tests for csv parsing to dto
```cs
 public class CsvServiceParseTests
{
    private readonly CsvService _csvService;
    private readonly Mock<IFormFile> _mockFile;

    public CsvServiceParseTests()
    {
        _csvService = new CsvService();
        _mockFile = new Mock<IFormFile>();
    }

    [Theory]
    [InlineData(',')]
    [InlineData(';')]
    public void ParseCsvFile_WithValidFile_ReturnsExpectedRecords(char delimiter)
    {
        // Arrange
        var mockFile = CreateMockCsvFile(GetValidCsvContent(delimiter), "products.csv");

        // Act
        var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var products = result.ToList();

        // First product validation
        products[0].SKU.Should().Be("1000001123");
        products[0].EAN.Should().Be("1234567890123");
        products[0].Name.Should().Be("Product 1");
        products[0].Price.Should().Be(10.99m);
        products[0].VAT.Should().Be(23);
        products[0].Quantity.Should().Be(100);
        products[0].Location.Should().Be("Warehouse A");
        products[0].Description.Should().Be("Description 1");
        products[0].AdditionalDescription.Should().Be("Additional info 1");
        products[0].Manufacturer.Should().Be("Manufacturer 1");
        products[0].Category.Should().Be("Category 1");

        products[0].Parameters.Should().ContainKey("Color");
        products[0].Parameters["Color"].Should().Be("Red");
        products[0].Parameters.Should().ContainKey("Size");
        products[0].Parameters["Size"].Should().Be("M");

        products[0].Images.Should().HaveCount(2);
        products[0].Images.Should().Contain("image1_1.jpg");
        products[0].Images.Should().Contain("image1_2.jpg");

        //second product validation
        products[1].SKU.Should().Be("3000001123");
        products[1].EAN.Should().BeNull();
        products[1].Name.Should().Be("Product 2");
        products[1].Price.Should().Be(20.99m);
        products[1].VAT.Should().Be(8);
        products[1].Quantity.Should().BeNull();
        products[1].Location.Should().Be("Warehouse B");
        products[1].Description.Should().Be("Description 2");
        products[1].AdditionalDescription.Should().BeNull();
        products[1].Manufacturer.Should().Be("Manufacturer 2");
        products[1].Category.Should().Be("Category 2");

        products[1].Parameters.Should().ContainKey("Material");
        products[1].Parameters["Material"].Should().Be("Cotton");

        products[1].Images.Should().HaveCount(1);
        products[1].Images.Should().Contain("image2_1.jpg");
    }

    [Fact]
    public void ParseCsvFile_WithEmptyRequiredField_ThrowsCsvHelperBadDataException()
    {
        // Arrange
        var delimiter = ',';
        var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images\n" +
                         ",1234567890123,Product 1,10.99,23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg";
        var mockFile = CreateMockCsvFile(csvContent, "products.csv");

        // Act
        Action action = () => _csvService.ParseCsvFile(mockFile.Object, delimiter);

        //Assert
        action.Should().Throw<CsvHelperBadDataException>();
    }

    [Fact]
    public void ParseCsvFile_WithInvalidPrice_ThrowsCsvHelperBadDataException()
    {
        // Arrange
        var delimiter = ',';
        var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images\n" +
                         "SKU001,1234567890123,Product 1,invalid,23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg";
        var mockFile = CreateMockCsvFile(csvContent, "products.csv");

        // Act
        Action action = () => _csvService.ParseCsvFile(mockFile.Object, delimiter);

        //Assert
        action.Should().Throw<CsvHelperBadDataException>();
    }

    [Fact]
    public void ParseCsvFile_WithCustomParameters_ParsesParametersCorrectly()
    {
        // Arrange
        var delimiter = ',';
        var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images,Color,Size,Weight\n" +
                         "SKU0000001,1234567890123,Product 1,\"10,99\",23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg,Red,M,500g";
        var mockFile = CreateMockCsvFile(csvContent, "products.csv");

        // Act
        var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var product = result.First();
        product.Parameters.Should().ContainKey("Color");
        product.Parameters["Color"].Should().Be("Red");
        product.Parameters.Should().ContainKey("Size");
        product.Parameters["Size"].Should().Be("M");
        product.Parameters.Should().ContainKey("Weight");
        product.Parameters["Weight"].Should().Be("500g");
    }

    [Fact]
    public void ParseCsvFile_WithEmptyOptionalField_ParsesSuccessfully()
    {
        // Arrange
        var delimiter = ',';
        var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images,Size\n" +
                         "PRODUCT0001,,Product One,\"10,99\",23,,,This is a detailed description,,,,image1_1.jpg,43";
        var mockFile = CreateMockCsvFile(csvContent, "products.csv");

        // Act
        var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var product = result.First();
        product.EAN.Should().BeNull();
        product.Quantity.Should().BeNull();
        product.AdditionalDescription.Should().BeNull();
    }

    private Mock<IFormFile> CreateMockCsvFile(string csvContent, string fileName)
    {
        var bytes = Encoding.UTF8.GetBytes(csvContent);
        var stream = new MemoryStream(bytes);

        _mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
        _mockFile.Setup(f => f.FileName).Returns(fileName);
        _mockFile.Setup(f => f.Length).Returns(bytes.Length);

        return _mockFile;
    }

    private string GetValidCsvContent(char delimiter)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"SKU{delimiter}EAN{delimiter}Name{delimiter}Price{delimiter}VAT{delimiter}Quantity{delimiter}Location{delimiter}Description{delimiter}AdditionalDescription{delimiter}Manufacturer{delimiter}Category{delimiter}Images{delimiter}Color{delimiter}Size{delimiter}Material");

        stringBuilder.AppendLine($"1000001123{delimiter}1234567890123{delimiter}Product 1{delimiter}\"10,99\"{delimiter}23{delimiter}100{delimiter}Warehouse A{delimiter}Description 1{delimiter}Additional info 1{delimiter}Manufacturer 1{delimiter}Category 1{delimiter}\"image1_1.jpg,image1_2.jpg\"{delimiter}Red{delimiter}M{delimiter}");

        stringBuilder.AppendLine($"3000001123{delimiter}{delimiter}Product 2{delimiter}\"20,99\"{delimiter}8{delimiter}{delimiter}Warehouse B{delimiter}Description 2{delimiter}{delimiter}Manufacturer 2{delimiter}Category 2{delimiter}image2_1.jpg{delimiter}{delimiter}{delimiter}Cotton");

        return stringBuilder.ToString();
    }
}
```


## Integration tests

For integration tests, also written using the xUnit framework, I used WebApplicationFactory to spin up the API in memory. However, instead of relying on EF Core's in-memory database—which can be convenient but potentially unreliable for real-world scenarios—I opted to use Testcontainers. This approach allows tests to run against a real database instance within Docker, ensuring higher fidelity and more accurate test outcomes.

The BaseTestApp class is responsible for managing the lifecycle of these test containers, including database initialization and disposal. While using Testcontainers introduces additional complexity and verbosity compared to in-memory solutions, it's a necessary trade-off to ensure the reliability and validity of the integration tests.

BaseTestApp:
```cs
 public class BaseTestApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
    private readonly FakeTimeProvider _fakeTimeProvider;
    public BaseTestApp()
    {
        _fakeTimeProvider = new FakeTimeProvider();
        _fakeTimeProvider.SetUtcNow(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsInterface && x != typeof(DbContext));

            foreach (var dbContextType in dbContextTypes)
            {
                var dbContextOptionsType = typeof(DbContextOptions<>).MakeGenericType(dbContextType);

                var descriptor = services.SingleOrDefault(s => s.ServiceType == dbContextOptionsType);
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                Type dbContextOptionsBuilderType = typeof(DbContextOptionsBuilder<>);
                Type dbContextOptionsBuilderGenericType = dbContextOptionsBuilderType.MakeGenericType(dbContextType);
                DbContextOptionsBuilder dbContextOptionsBuilderInstance = Activator.CreateInstance(dbContextOptionsBuilderGenericType) as DbContextOptionsBuilder ?? throw new NullReferenceException();
                dbContextOptionsBuilderInstance.UseNpgsql(_dbContainer.GetConnectionString());
                var options = dbContextOptionsBuilderInstance.Options;

                services.AddScoped(dbContextOptionsType, _ => options);
                services.AddScoped(dbContextType, sp =>
                {
                    var options = sp.GetRequiredService(dbContextOptionsType);
                    return (DbContext?)Activator.CreateInstance(dbContextType, options, _fakeTimeProvider) ?? throw new NullReferenceException(); ;
                });
            }

            services.RemoveAll<TimeProvider>();
            var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
            services.AddSingleton<TimeProvider>(fakeTimeProvider);

            ConfigureTestServices(services);
        });
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        // Empty by default, to be overridden by derived classes if needed
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
```
Base controller test class:
```cs
public class ControllerTests : IClassFixture<EcommerceTestApp>
{
    internal readonly InventoryDbContext InventoryDbContext;
    protected readonly HttpClient HttpClient;
    protected readonly string BaseEndpoint = "/api/v1/inventory-module/";
    private readonly EcommerceTestApp _ecommerceTestApp;

    public ControllerTests(EcommerceTestApp ecommerceTestApp)
    {
        _ecommerceTestApp = ecommerceTestApp;
        HttpClient = _ecommerceTestApp.CreateClient();
        var scope = _ecommerceTestApp.Services.CreateScope();
        InventoryDbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        if (InventoryDbContext.Database.GetPendingMigrations().Any())
        {
            InventoryDbContext.Database.Migrate();
        }
    }

    protected void Authorize()
    {
        var jwt = AuthHelper.CreateToken(Guid.NewGuid().ToString(), "username", "Admin");
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
```

Integration tests example:
```cs
public class InventoryControllerCreateProductTests : ControllerTests
{
    private readonly string _controllerName = "Products";
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public InventoryControllerCreateProductTests(EcommerceTestApp ecommerceTestApp, ITestOutputHelper testOutputHelper) : base(ecommerceTestApp)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateProduct_WithCorrectData_ShouldReturn201AndId()
    {
        //Arrange
        var (categoryId, manufacturerId, parameterId) = await Seed();
        var command = new CreateProduct();
        using var formContent = new MultipartFormDataContent
        {
            { new StringContent("12345678"), nameof(command.SKU) },
            { new StringContent("name"), nameof(command.Name) },
            { new StringContent(5.ToString()), nameof(command.Price) },
            { new StringContent(23.ToString()), nameof(command.VAT) },
            { new StringContent("description"), nameof(command.Description) },
            { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
            { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
        };
        var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
        {
            ParameterId = parameterId,
            Value = "value"
        });
        formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));
        var imageStream = new MemoryStream();
        var imageContent = new StreamContent(imageStream);
        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        formContent.Add(imageContent, "image", "image.jpg");

        //Act
        Authorize();
        var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

        //Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        var productCreateApiResponse = JsonSerializer.Deserialize<ApiResponseTest<CreateProductData>>(httpContent, _jsonSerializerOptions);
        productCreateApiResponse.Should().NotBeNull();
        productCreateApiResponse.Code.Should().Be(HttpStatusCode.Created);
        productCreateApiResponse.Status.Should().Be("success");
        productCreateApiResponse.Data.Should().NotBeNull();
        var product = await InventoryDbContext.Products.FirstOrDefaultAsync(c => c.Id == productCreateApiResponse.Data.Id);
        product.Should().NotBeNull();

    }

    [Fact]
    public async Task CreateProduct_WithNotExistingCategory_ShouldReturn400AndErrorMessage()
    {
        //Arrange
        var (_, manufacturerId, parameterId) = await Seed();
        var categoryId = Guid.NewGuid();
        var command = new CreateProduct();
        using var formContent = new MultipartFormDataContent
        {
            { new StringContent("12345678"), nameof(command.SKU) },
            { new StringContent("name"), nameof(command.Name) },
            { new StringContent(5.ToString()), nameof(command.Price) },
            { new StringContent(23.ToString()), nameof(command.VAT) },
            { new StringContent("description"), nameof(command.Description) },
            { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
            { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
        };
        var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
        {
            ParameterId = parameterId,
            Value = "value"
        });
        formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

        //Act
        Authorize();
        var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);
        
        //Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
        productCreateExceptionResponse.Should().NotBeNull();
        productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
        productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
        productCreateExceptionResponse.Detail.Should().Be($"Category: {categoryId} was not found.");
    }

    [Fact]
    public async Task CreateProduct_WithNotExistingManufacturer_ShouldReturn400AndErrorMessage()
    {
        //Arrange
        var (categoryId, _, parameterId) = await Seed();
        var manufacturerId = Guid.NewGuid();
        var command = new CreateProduct();
        using var formContent = new MultipartFormDataContent
        {
            { new StringContent("12345678"), nameof(command.SKU) },
            { new StringContent("name"), nameof(command.Name) },
            { new StringContent(5.ToString()), nameof(command.Price) },
            { new StringContent(23.ToString()), nameof(command.VAT) },
            { new StringContent("description"), nameof(command.Description) },
            { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
            { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
        };
        var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
        {
            ParameterId = parameterId,
            Value = "value"
        });
        formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

        //Act
        Authorize();
        var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

        //Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
        productCreateExceptionResponse.Should().NotBeNull();
        productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
        productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
        productCreateExceptionResponse.Detail.Should().Be($"Manufacturer: {manufacturerId} was not found.");
    }

    [Fact]
    public async Task CreateProduct_WithNotExistingParameter_ShouldReturn400AndErrorMessage()
    {
        //Arrange
        var (categoryId, manufacturerId, _) = await Seed();
        var parameterId = Guid.NewGuid();
        var command = new CreateProduct();
        using var formContent = new MultipartFormDataContent
        {
            { new StringContent("12345678"), nameof(command.SKU) },
            { new StringContent("name"), nameof(command.Name) },
            { new StringContent(5.ToString()), nameof(command.Price) },
            { new StringContent(23.ToString()), nameof(command.VAT) },
            { new StringContent("description"), nameof(command.Description) },
            { new StringContent(manufacturerId.ToString()), nameof(command.ManufacturerId) },
            { new StringContent(categoryId.ToString()), nameof(command.CategoryId) }
        };
        var parametersJson = JsonSerializer.Serialize(new ProductParameterDto()
        {
            ParameterId = parameterId,
            Value = "value"
        });
        formContent.Add(new StringContent(parametersJson, Encoding.UTF8, "application/json"), nameof(command.ProductParameters));

        //Act
        Authorize();
        var httpResponse = await HttpClient.PostAsync(BaseEndpoint + _controllerName, formContent);

        //Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var httpContent = await httpResponse.Content.ReadAsStringAsync();
        var productCreateExceptionResponse = JsonSerializer.Deserialize<ExceptionResponseTest>(httpContent, _jsonSerializerOptions);
        productCreateExceptionResponse.Should().NotBeNull();
        productCreateExceptionResponse.Status.Should().Be(HttpStatusCode.BadRequest);
        productCreateExceptionResponse.Title.Should().Be("An exception occurred.");
        productCreateExceptionResponse.Detail.Should().Be($"Parameter: {parameterId} was not found.");
    }

    public async Task<(Guid CategoryId, Guid ManufacturerId, Guid ParameterId)> Seed()
    {
        var category = new Category("category");
        var manufacturer = new Manufacturer("manufacturer");
        var parameter = new Parameter("parameter");
        var categoryEntry = await InventoryDbContext.AddAsync(category);
        var parameterEntry = await InventoryDbContext.AddAsync(parameter);
        var manufacturerEntry = await InventoryDbContext.AddAsync(manufacturer);
        await InventoryDbContext.SaveChangesAsync();
        return (categoryEntry.Entity.Id, manufacturerEntry.Entity.Id, parameterEntry.Entity.Id);
    }
}
```

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
- [x] Validate CSV product imports.
- [ ] Autocomplete of customer's details when logged in.
- [ ] Make discount Id as type Guid to facilite integration with carts module.
- [ ] Put submit return and complaint in it's respective controllers, instead of having it in OrdersController.
- [ ] Make API more resilient to errors by implementing rollbacks for extenal services such as Stripe, InPost or Azurite.
- [ ] Add raports for number of order, returns, complaints and sold products per day, month and year endpoints.
- [ ] Enable creating decisions for complaints as drafts.

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
