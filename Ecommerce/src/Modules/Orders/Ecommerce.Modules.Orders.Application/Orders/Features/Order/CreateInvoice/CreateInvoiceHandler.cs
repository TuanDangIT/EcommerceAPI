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

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.CreateInvoice
{
    internal class CreateInvoiceHandler : ICommandHandler<CreateInvoice, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IOrderInvoiceCreationPolicy _orderInvoiceCreationPolicy;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IMessageBroker _messageBroker;
        private readonly IOrdersEventMapper _ordersEventMapper;
        private readonly CompanyOptions _companyOptions;
        private readonly TimeProvider _timeProvider;
        private const string _invoiceTemplatePath = "Invoices\\InvoiceTemplates\\Invoice.html";
        private const string _containerName = "invoices";

        public CreateInvoiceHandler(IOrderRepository orderRepository, IBlobStorageService blobStorageService, IOrderInvoiceCreationPolicy orderInvoiceCreationPolicy, 
            IDomainEventDispatcher domainEventDispatcher, IMessageBroker messageBroker, IOrdersEventMapper ordersEventMapper, CompanyOptions companyOptions, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
            _orderInvoiceCreationPolicy = orderInvoiceCreationPolicy;
            _domainEventDispatcher = domainEventDispatcher;
            _messageBroker = messageBroker;
            _ordersEventMapper = ordersEventMapper;
            _companyOptions = companyOptions;
            _timeProvider = timeProvider;
        }
        public async Task<string> Handle(CreateInvoice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderAsync(request.OrderId) ?? throw new OrderNotFoundException(request.OrderId);
            if(!(await _orderInvoiceCreationPolicy.CanCreateInvoice(order)))
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
            var file = new FormFile(stream, 0, stream.Length, "invoice", invoiceNo)/* { ContentType = "application/pdf" }*/
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = "application/pdf";
            await _blobStorageService.UploadAsync(file, invoiceNo, _containerName);
            var domainEvent = new InvoiceCreated(order.Id, order.Customer.UserId, invoiceNo, order.Customer.FirstName, order.Customer.Email);
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            //var integrationEvent = await _ordersEventMapper.MapAsync(domainEvent);
            var integrationEvent = _ordersEventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
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
            invoiceTemplate = invoiceTemplate.Replace("{customerAddress}", order.Shipment.Receiver.Address.Street);
            invoiceTemplate = invoiceTemplate.Replace("{customerAddressNumber}", order.Shipment.Receiver.Address.BuildingNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerCountry}", "Poland");
            invoiceTemplate = invoiceTemplate.Replace("{customerPostCode}", order.Shipment.Receiver.Address.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerCity}", order.Shipment.Receiver.Address.City);
            invoiceTemplate = invoiceTemplate.Replace("{customerEmail}", order.Customer.Email);
            invoiceTemplate = invoiceTemplate.Replace("{customerPhoneNumber}", order.Customer.PhoneNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerName}", order.Customer.FirstName + " " + order.Customer.LastName);
            invoiceTemplate = invoiceTemplate.Replace("{additionalInformation}", order.CompanyAdditionalInformation ?? "");
            invoiceTemplate = invoiceTemplate.Replace("{totalPrice}", order.TotalSum.ToString());
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
