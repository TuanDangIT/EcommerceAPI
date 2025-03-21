﻿using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
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
using Microsoft.Extensions.Logging;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice
{
    internal class CreateInvoiceHandler : ICommandHandler<CreateInvoice, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMessageBroker _messageBroker;
        private readonly CompanyOptions _companyOptions;
        private readonly StripeOptions _stripeOptions;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CreateInvoiceHandler> _logger;
        private readonly IContextService _contextService;
        private const string _invoiceTemplatePath = "Orders\\InvoiceTemplates\\Invoice.html";
        private const string _containerName = "invoices";
        private readonly string _contentType = "application/pdf";

        public CreateInvoiceHandler(IOrderRepository orderRepository, IBlobStorageService blobStorageService,
            IMessageBroker messageBroker, CompanyOptions companyOptions, StripeOptions stripeOptions, IInvoiceRepository invoiceRepository, TimeProvider timeProvider,
            ILogger<CreateInvoiceHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
            _messageBroker = messageBroker;
            _companyOptions = companyOptions;
            _stripeOptions = stripeOptions;
            _invoiceRepository = invoiceRepository;
            _timeProvider = timeProvider;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<string> Handle(CreateInvoice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken,
                query => query.Include(o => o.Invoice),
                query => query.Include(o => o.Customer)) ?? 
                throw new OrderNotFoundException(request.OrderId);
            if(order.Status == OrderStatus.Draft)
            {
                throw new OrderDraftException(order.Id);
            }
            if(order.Status != OrderStatus.Placed && order.Status != OrderStatus.ParcelPacked)
            {
                throw new InvoiceCannotCreateInvoiceException(order.Id, order.Status.ToString());
            }
            if (order.HasInvoice)
            {
                throw new InvoiceAlreadyCreatedException(order.Id);
            }
            var now = _timeProvider.GetUtcNow();
            Random rand = new();
            var invoiceNo = string.Concat(now.Year, "/", now.Month, "/", now.Millisecond, now.Nanosecond, rand.Next(0, 9));
            var invoiceTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _invoiceTemplatePath));
            invoiceTemplate = FillInvoiceDetails(invoiceTemplate, invoiceNo, order);
            HtmlToPdf converter = new();
            PdfDocument pdf = converter.ConvertHtmlString(invoiceTemplate);
            var stream = new MemoryStream();
            pdf.Save(stream);
            var file = new FormFile(stream, 0, stream.Length, "invoice", invoiceNo)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            await _blobStorageService.UploadAsync(file, invoiceNo, _containerName, cancellationToken);
            await _invoiceRepository.CreateAsync(new Domain.Orders.Entities.Invoice(invoiceNo, order));
            _logger.LogInformation("Invoice: {invoiceNo} was created for order: {orderId} by {@user}.", invoiceNo, order.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new InvoiceCreated(order.Id, order.Customer.UserId, order.Customer.FirstName, order.Customer.Email, invoiceNo));
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
            invoiceTemplate = invoiceTemplate.Replace("{customerCountry}", order.Customer.Address.Country);
            invoiceTemplate = invoiceTemplate.Replace("{customerPostCode}", order.Customer.Address.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerCity}", order.Customer.Address.City);
            invoiceTemplate = invoiceTemplate.Replace("{customerEmail}", order.Customer.Email);
            invoiceTemplate = invoiceTemplate.Replace("{customerPhoneNumber}", order.Customer.PhoneNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerName}", order.Customer.FirstName + " " + order.Customer.LastName);
            invoiceTemplate = invoiceTemplate.Replace("{additionalInformation}", order.CompanyAdditionalInformation ?? "");
            invoiceTemplate = invoiceTemplate.Replace("{totalPrice}", order.TotalSum.ToString("0.00") + " " + _stripeOptions.Currency);
            invoiceTemplate = invoiceTemplate.Replace("{shipPrice}", order.ShippingPrice.ToString("0.00"));
            StringBuilder productsHtml = new StringBuilder();
            foreach (var product in order.Products)
            {
                productsHtml.Append(
                    $"""
                        <tr>
                            <td>{product.Name}</td>
                            <td>{product.SKU}</td>
                            <td>{product.Price.ToString("0.00")}</td>
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
