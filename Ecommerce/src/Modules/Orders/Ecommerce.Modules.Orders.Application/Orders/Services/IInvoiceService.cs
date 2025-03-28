using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    public interface IInvoiceService
    {
        Task<(string InvoiceNo, FormFile File)> CreateInvoiceAsync(Order order);
    }
}
