using Ecommerce.Modules.Orders.Domain.Invoices.Entities;
using Ecommerce.Modules.Orders.Domain.Invoices.Repositories;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Invoices.Events.Handlers
{
    internal class InvoiceCreatedHandler : IDomainEventHandler<InvoiceCreated>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;

        public InvoiceCreatedHandler(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
        }
        public async Task HandleAsync(InvoiceCreated @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId) ?? throw new OrderNotFoundException(@event.OrderId);
            var customer = new InvoiceCustomer(@event.Customer.FirstName, @event.Customer.LastName, @event.Customer.Email, @event.Customer.PhoneNumber);
            await _invoiceRepository.CreateAsync(new Invoice(@event.InvoiceNo, order, customer));
        }
    }
}
