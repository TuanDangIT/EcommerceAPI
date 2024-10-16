using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.DeleteInvoice
{
    public sealed record class DeleteInvoice(int InvoiceId) : ICommand;
}
