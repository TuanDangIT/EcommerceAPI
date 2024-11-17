using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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

        public DeleteInvoiceHandler(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
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
            await _invoiceRepository.DeleteAsync(invoice.Id);
        }
    }
}
