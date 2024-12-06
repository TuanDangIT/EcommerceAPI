using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DeleteInvoice
{
    internal class DeleteInvoiceHandler : ICommandHandler<DeleteInvoice>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DeleteInvoiceHandler> _logger;
        private readonly IContextService _contextService;
        public readonly string _containerName = "invoices";

        public DeleteInvoiceHandler(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository, IBlobStorageService blobStorageService
            ,ILogger<DeleteInvoiceHandler> logger, IContextService contextService)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteInvoice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var invoice = order.Invoice;
            if(invoice is null)
            {
                throw new OrderInvoiceNotFoundException(request.OrderId);
            }
            await _blobStorageService.DeleteAsync(invoice.InvoiceNo, _containerName);
            await _invoiceRepository.DeleteAsync(invoice.Id);
            _logger.LogInformation("Invoice: {invoice} was deleted by {username}:{userId}.", invoice, _contextService.Identity!.Username, _contextService.Identity!.Id);

        }
    }
}
