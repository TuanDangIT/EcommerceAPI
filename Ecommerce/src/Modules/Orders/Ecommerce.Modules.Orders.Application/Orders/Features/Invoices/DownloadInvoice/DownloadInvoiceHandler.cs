using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.MediatR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Invoice.DownloadInvoice
{
    internal class DownloadInvoiceHandler : IQueryHandler<DownloadInvoice, (Stream FileStream, string MimeType, string FileName)>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBlobStorageService _blobStorageService;
        private const string _containerName = "invoices";
        private const string _mimeType = "application/pdf";

        public DownloadInvoiceHandler(IOrderRepository orderRepository, IBlobStorageService blobStorageService)
        {
            _orderRepository = orderRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<(Stream FileStream, string MimeType, string FileName)> Handle(DownloadInvoice request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            if (order.Invoice is null)
            {
                throw new OrderCannotDownloadInvoiceException(request.OrderId);
            }
            var dto = await _blobStorageService.DownloadAsync(order.Invoice.InvoiceNo, _containerName);
            return (dto.FileStream, _mimeType, $"{order.Invoice.InvoiceNo}-invoice");
        }
    }
}
