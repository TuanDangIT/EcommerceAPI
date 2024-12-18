using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts
{
    public sealed record class ImportProducts(IFormFile ImportFile, char Delimiter) : ICommand;
}
