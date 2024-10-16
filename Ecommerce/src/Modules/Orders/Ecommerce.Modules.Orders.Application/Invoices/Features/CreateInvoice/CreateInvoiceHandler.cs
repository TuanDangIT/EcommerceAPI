using Ecommerce.Shared.Abstractions.MediatR;
using MediatR;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Features.CreateInvoice
{
    //internal class CreateInvoiceHandler : ICommandHandler<CreateInvoice, byte[]>
    //{
    //    private const string _invoiceTemplatePath = "Invoices\\InvoiceTemplates\\InvoiceTemplate.html";
    //    public CreateInvoiceHandler()
    //    {
            
    //    }

    //    public Task<byte[]> Handle(CreateInvoice request, CancellationToken cancellationToken)
    //    {
    //        Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _invoiceTemplatePath));
    //        var invoiceTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _invoiceTemplatePath));
    //        HtmlToPdf converter = new HtmlToPdf();
    //        PdfDocument pdf = converter.ConvertHtmlString(invoiceTemplate);
    //        return Task.FromResult(pdf.Save());
    //    }
    //}
}
