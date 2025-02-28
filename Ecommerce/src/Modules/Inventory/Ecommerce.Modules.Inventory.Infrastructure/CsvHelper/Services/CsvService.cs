using Azure.Core;
using CsvHelper.Configuration;
using CsvHelper;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts;
using Ecommerce.Modules.Inventory.Application.Shared.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.CsvHelper.Services
{
    internal class CsvService : ICsvService
    {
        public IEnumerable<ProductCsvRecordDto> ParseCsvFile(IFormFile file, char delimiter)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
                Delimiter = delimiter.ToString(),
                BadDataFound = null
            };

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap(new ProductCsvClassMap(delimiter is ',' ? ';' : ','));
            return csv.GetRecords<ProductCsvRecordDto>().ToList();
        }
    }
}
