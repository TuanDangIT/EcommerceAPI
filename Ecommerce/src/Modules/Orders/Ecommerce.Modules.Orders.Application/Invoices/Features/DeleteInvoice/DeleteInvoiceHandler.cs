using Ecommerce.Modules.Orders.Domain.Invoices.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.DeleteInvoice
{
    internal class DeleteInvoiceHandler : ICommandHandler<DeleteInvoice>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public DeleteInvoiceHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task Handle(DeleteInvoice request, CancellationToken cancellationToken)
            => await _invoiceRepository.DeleteAsync(request.InvoiceId);
    }
}
