using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
//using SelectPdf;
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
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.CreateInvoice
{
    internal class CreateInvoiceHandler : ICommandHandler<CreateInvoice, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMessageBroker _messageBroker;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<CreateInvoiceHandler> _logger;
        private readonly IContextService _contextService;
        private readonly IInvoiceService _invoiceService;
        private const string _containerName = "invoices";

        public CreateInvoiceHandler(IOrderRepository orderRepository, IBlobStorageService blobStorageService,
            IMessageBroker messageBroker, IInvoiceRepository invoiceRepository, ILogger<CreateInvoiceHandler> logger, 
            IContextService contextService, IInvoiceService invoiceService)
        {
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
            _messageBroker = messageBroker;
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _contextService = contextService;
            _invoiceService = invoiceService;
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
            (var invoiceNo, var file) = await _invoiceService.CreateInvoiceAsync(order);
            await _blobStorageService.UploadAsync(file, invoiceNo, _containerName, cancellationToken);
            await _invoiceRepository.CreateAsync(new Domain.Orders.Entities.Invoice(invoiceNo, order));
            _logger.LogInformation("Invoice: {invoiceNo} was created for order: {orderId} by {@user}.", invoiceNo, order.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new InvoiceCreated(order.Id, order.Customer!.UserId, order.Customer.FirstName, order.Customer.Email, invoiceNo));
            return invoiceNo;
        }
    }
}
