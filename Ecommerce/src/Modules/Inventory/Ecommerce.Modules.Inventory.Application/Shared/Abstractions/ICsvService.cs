using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Shared.Abstractions
{
    public interface ICsvService
    {
        IEnumerable<ProductCsvRecordDto> ParseCsvFile(IFormFile file, char delimiter);
    }
}
