using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ecommerce.Modules.Orders.Domain.Orders.Policies;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Shared.Infrastructure.Stripe;
using Ecommerce.Modules.Orders.Application.Orders.Events;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice
{
    internal class CreateInvoiceHandler : ICommandHandler<CreateInvoice, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IOrderInvoiceCreationPolicy _orderInvoiceCreationPolicy;
        private readonly IMessageBroker _messageBroker;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly TimeProvider _timeProvider;
        private const string _invoiceTemplatePath = "Orders\\InvoiceTemplates\\Invoice.html";
        private const string _containerName = "invoices";
        private readonly decimal _defaultDeliveryPrice = 15;
        private readonly string _contentType = "application/pdf";

        public CreateInvoiceHandler(IOrderRepository orderRepository, IBlobStorageService blobStorageService, IOrderInvoiceCreationPolicy orderInvoiceCreationPolicy,
            IMessageBroker messageBroker, CompanyOptions companyOptions, StripeOptions stripeOptions, IInvoiceRepository invoiceRepository, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
            _orderInvoiceCreationPolicy = orderInvoiceCreationPolicy;
            _messageBroker = messageBroker;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
            _invoiceRepository = invoiceRepository;
            _timeProvider = timeProvider;
        }
        public async Task<string> Handle(CreateInvoice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId) ?? throw new OrderNotFoundException(request.OrderId);
            if (!await _orderInvoiceCreationPolicy.CanCreateInvoice(order))
            {
                throw new OrderInvoiceAlreadyCreatedException(order.Id);
            }
            var now = _timeProvider.GetUtcNow();
            Random rand = new Random();
            var invoiceNo = string.Concat(now.Year, "/", now.Month, "/", now.Millisecond, now.Nanosecond, rand.Next(0, 9));
            var invoiceTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _invoiceTemplatePath));
            invoiceTemplate = FillInvoiceDetails(invoiceTemplate, invoiceNo, order);
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument pdf = converter.ConvertHtmlString(invoiceTemplate);
            var stream = new MemoryStream();
            pdf.Save(stream);
            var file = new FormFile(stream, 0, stream.Length, "invoice", invoiceNo)
            {
                //ContentType = _contentType,
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            await _blobStorageService.UploadAsync(file, invoiceNo, _containerName);
            await _invoiceRepository.CreateAsync(new Domain.Orders.Entities.Invoice(invoiceNo, order));
            await _messageBroker.PublishAsync(new InvoiceCreated(order.Id, order.Customer.Id, order.Customer.FirstName, order.Customer.Email, invoiceNo));
            return invoiceNo;
        }
        private string FillInvoiceDetails(string invoiceTemplate, string invoiceNo, Domain.Orders.Entities.Order order)
        {
            invoiceTemplate = invoiceTemplate.Replace("{invoiceId}", invoiceNo);
            invoiceTemplate = invoiceTemplate.Replace("{companyName}", _companyOptions.Name);
            invoiceTemplate = invoiceTemplate.Replace("{companyAddress}", _companyOptions.Address);
            invoiceTemplate = invoiceTemplate.Replace("{companyCountry}", _companyOptions.Country);
            invoiceTemplate = invoiceTemplate.Replace("{companyCity}", _companyOptions.City);
            invoiceTemplate = invoiceTemplate.Replace("{companyPostCode}", _companyOptions.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerAddress}", order.Customer.Address.Street);
            invoiceTemplate = invoiceTemplate.Replace("{customerAddressNumber}", order.Customer.Address.BuildingNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerCountry}", "Poland");
            invoiceTemplate = invoiceTemplate.Replace("{customerPostCode}", order.Customer.Address.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerCity}", order.Customer.Address.City);
            invoiceTemplate = invoiceTemplate.Replace("{customerEmail}", order.Customer.Email);
            invoiceTemplate = invoiceTemplate.Replace("{customerPhoneNumber}", order.Customer.PhoneNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerName}", order.Customer.FirstName + " " + order.Customer.LastName);
            invoiceTemplate = invoiceTemplate.Replace("{additionalInformation}", order.CompanyAdditionalInformation ?? "");
            invoiceTemplate = invoiceTemplate.Replace("{totalPrice}", order.TotalSum.ToString() + " " + _stripeOptions.Currency);
            invoiceTemplate = invoiceTemplate.Replace("{shipPrice}", _defaultDeliveryPrice + " " + _stripeOptions.Currency);
            StringBuilder productsHtml = new StringBuilder();
            foreach (var product in order.Products)
            {
                productsHtml.Append(
                    $"""
                        <tr>
                            <td>{product.Name}</td>
                            <td>{product.SKU}</td>
                            <td>{product.Price}</td>
                            <td>{product.Quantity}</td>
                        </tr>
                    """
                    );
            }
            invoiceTemplate = invoiceTemplate.Replace("{orderItems}", productsHtml.ToString());
            return invoiceTemplate;
        }
    }
}
