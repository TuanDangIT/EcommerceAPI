using Ecommerce.Modules.Orders.Application.Invoices.Exceptions;
using Ecommerce.Modules.Orders.Domain.Invoices.Entities;
using Ecommerce.Modules.Orders.Domain.Invoices.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.EditCustomer
{
    internal class EditCustomerHandler : ICommandHandler<EditCustomer>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public EditCustomerHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task Handle(EditCustomer request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetAsync(request.InvoiceId) ?? throw new InvoiceNotFoundException(request.InvoiceId);
            invoice.EditCustomer(new InvoiceCustomer(request.Customer.FirstName, request.Customer.LastName, request.Customer.Email, request.Customer.PhoneNumber));
            await _invoiceRepository.UpdateAsync();
        }
    }
}
