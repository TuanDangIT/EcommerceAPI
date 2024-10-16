using Ecommerce.Modules.Orders.Application.Invoices.Exceptions;
using Ecommerce.Modules.Orders.Domain.Invoices.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.DownloadInvoice
{
    internal class DownloadInvoiceHandler : ICommandHandler<DownloadInvoice, (Stream FileStream, string MimeType, string FileName)>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBlobStorageService _blobStorageService;
        private const string _containerName = "invoices";
        private const string _mimeType = "application/pdf";

        public DownloadInvoiceHandler(IInvoiceRepository invoiceRepository, IBlobStorageService blobStorageService)
        {
            _invoiceRepository = invoiceRepository;
            _blobStorageService = blobStorageService;
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> Handle(DownloadInvoice request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetAsync(request.InvoiceId) ?? throw new InvoiceNotFoundException(request.InvoiceId);
            var dto = await _blobStorageService.DownloadAsync(invoice.InvoiceNo, _containerName);
            return (dto.FileStream, _mimeType, $"{invoice.InvoiceNo}-invoice");
        }
    }
}
