using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    internal class OrdersEventMapper : IOrdersEventMapper
    {
        private readonly IBlobStorageService _blobStorageService;
        private const string _containerName = "invoices";

        public OrdersEventMapper(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }
        public IMessage Map(IDomainEvent @event)
            => @event switch
            {
                Domain.Orders.Events.InvoiceCreated e => new Events.InvoiceCreated(e.OrderId, e.CustomerId, e.FirstName, e.Email, e.InvoiceNo),
                Domain.Orders.Events.OrderReturned e => new OrderReturned(e.OrderId, e.CustomerId, e.FirstName, e.Email, e.Products.Select(p => new { p.SKU, p.Name, p.Price, p.Quantity })),
                Domain.Orders.Events.ComplaintSubmitted e => new ComplaintSubmitted(e.OrderId, e.CustomerId, e.FirstName, e.Email, e.Title, e.CreatedAt),
                _ => throw new ArgumentException(nameof(@event)),
            };
        //public async Task<IMessage> MapAsync(IDomainEvent @event)
        //{
        //    switch (@event)
        //    {
        //        case Domain.Orders.Events.InvoiceCreated e:
        //            var invoice = await _blobStorageService.DownloadAsync(e.InvoiceNo, _containerName);
        //            var stream = invoice.FileStream;
        //            return new InvoiceCreated(e.OrderId, e.FirstName, e.Email/*, new FormFile(stream, 0, stream.Length, "", e.InvoiceNo)*/);
        //        default:
        //            throw new ArgumentException(nameof(@event));
        //    }
        //}
    }
}
