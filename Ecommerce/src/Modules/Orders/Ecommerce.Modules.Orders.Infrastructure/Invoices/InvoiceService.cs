using ceTe.DynamicPDF.HtmlConverter;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Infrastructure.Company;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Invoices
{
    internal class InvoiceService : IInvoiceService
    {
        private readonly TimeProvider _timeProvider;
        private readonly CompanyOptions _companyOptions;
        private readonly ILogger<InvoiceService> _logger;
        private readonly StripeOptions _stripeOptions;
        private readonly string _invoiceTemplatePath = "Invoices/Templates/Invoice.html";
        private readonly string _contentType = "application/pdf";
        private readonly ConversionOptions _conversionOptions = new ConversionOptions(PageSize.A4, PageOrientation.Portrait, margins: 0);

        public InvoiceService(TimeProvider timeProvider, CompanyOptions companyOptions, ILogger<InvoiceService> logger,
            StripeOptions stripeOptions)
        {
            SetUpConverter();
            _timeProvider = timeProvider;
            _companyOptions = companyOptions;
            _logger = logger;
            _stripeOptions = stripeOptions;
        }

        public async Task<(string InvoiceNo, FormFile File)> CreateInvoiceAsync(Order order)
        {
            var invoiceNo = GenerateInvoiceNumber();
            var invoiceTemplate = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _invoiceTemplatePath));
            invoiceTemplate = FillInvoiceDetails(invoiceTemplate, invoiceNo, order);
            var bytes = await Converter.ConvertAsync(invoiceTemplate, conversionOptions: _conversionOptions) ?? 
                throw new HtmlNotConvertedException();
            var stream = new MemoryStream(bytes);
            var file = new FormFile(stream, 0, stream.Length, "invoice", invoiceNo)
            {
                Headers = new HeaderDictionary()
            };
            file.ContentType = _contentType;
            _logger.LogDebug("Invoice: {invoiceNo} was created for order: {orderId}", invoiceNo, order.Id);
            return (invoiceNo, file);
        }
        private void SetUpConverter()
        {
            Converter.ChromiumProcessPath = "/usr/bin/chromium";
            Converter.TemporaryDirectory = "/dpdfTemp";
        }
        private string GenerateInvoiceNumber()
        {
            var now = _timeProvider.GetUtcNow();
            return $"{now.Year}/{now.Month}/{now.Millisecond}{now.Nanosecond}{Random.Shared.Next(0, 9)}";
        }
        private string FillInvoiceDetails(string invoiceTemplate, string invoiceNo, Order order)
        {
            invoiceTemplate = invoiceTemplate.Replace("{invoiceId}", invoiceNo);
            invoiceTemplate = invoiceTemplate.Replace("{companyName}", _companyOptions.Name);
            invoiceTemplate = invoiceTemplate.Replace("{companyAddress}", _companyOptions.Address);
            invoiceTemplate = invoiceTemplate.Replace("{companyCountry}", _companyOptions.Country);
            invoiceTemplate = invoiceTemplate.Replace("{companyCity}", _companyOptions.City);
            invoiceTemplate = invoiceTemplate.Replace("{companyPostCode}", _companyOptions.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerAddress}", order.Customer!.Address.Street);
            invoiceTemplate = invoiceTemplate.Replace("{customerAddressNumber}", order.Customer.Address.BuildingNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerCountry}", order.Customer.Address.Country);
            invoiceTemplate = invoiceTemplate.Replace("{customerPostCode}", order.Customer.Address.PostCode);
            invoiceTemplate = invoiceTemplate.Replace("{customerCity}", order.Customer.Address.City);
            invoiceTemplate = invoiceTemplate.Replace("{customerEmail}", order.Customer.Email);
            invoiceTemplate = invoiceTemplate.Replace("{customerPhoneNumber}", order.Customer.PhoneNumber);
            invoiceTemplate = invoiceTemplate.Replace("{customerName}", order.Customer.FirstName + " " + order.Customer.LastName);
            invoiceTemplate = invoiceTemplate.Replace("{additionalInformation}", order.CompanyAdditionalInformation ?? "");
            invoiceTemplate = invoiceTemplate.Replace("{totalPrice}", order.TotalSum.ToString("0.00") + " " + _stripeOptions.Currency);
            invoiceTemplate = invoiceTemplate.Replace("{shipPrice}", order.ShippingPrice.ToString("0.00"));
            StringBuilder productsHtml = new StringBuilder();
            foreach (var product in order.Products)
            {
                productsHtml.Append(
                    $"""
                        <tr>
                            <td>{product.Name}</td>
                            <td>{product.SKU}</td>
                            <td>{product.Price.ToString("0.00")}</td>
                            <td>{product.Quantity}</td>
                        </tr>
                    """
                    );
            }
            invoiceTemplate = invoiceTemplate.Replace("{orderItems}", productsHtml.ToString());
            return invoiceTemplate;
        }
    }
}
