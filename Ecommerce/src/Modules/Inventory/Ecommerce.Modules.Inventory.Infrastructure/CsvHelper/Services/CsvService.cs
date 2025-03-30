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
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8,
                    HasHeaderRecord = true,
                    Delimiter = delimiter.ToString(),
                    //BadDataFound = args =>
                    //{
                    //    var currentIndex = args.Context.Reader?.CurrentIndex ?? throw new NullReferenceException();
                    //    var header = args.Context.Reader?.HeaderRecord![currentIndex]!;
                    //    var rowNumber = args.Context.Parser?.Row ?? throw new NullReferenceException();
                    //    var isFieldEmpty = args.Field == "";
                    //    if (isFieldEmpty)
                    //    {
                    //        throw new CsvHelperBadDataException(header, rowNumber, args.Field);
                    //    }
                    //    else
                    //    {
                    //        throw new CsvHelperBadDataException(header, rowNumber);
                    //    }
                    //},
                };

                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, config);

                csv.Context.RegisterClassMap(new ProductCsvClassMap(delimiter is ',' ? ';' : ','));
                return csv.GetRecords<ProductCsvRecordDto>().ToList();

            }catch(FieldValidationException ex)
            {
                var currentIndex = ex.Context?.Reader?.CurrentIndex ?? throw new NullReferenceException();
                var header = ex.Context.Reader?.HeaderRecord![currentIndex]!;
                var rowNumber = ex.Context.Parser?.Row - 1 ?? throw new NullReferenceException();
                var isFieldEmpty = ex.Field == "";
                if (!isFieldEmpty)
                {
                    throw new CsvHelperBadDataException(header, rowNumber, ex.Field);
                }
                else
                {
                    throw new CsvHelperBadDataException(header, rowNumber);
                }
            }
        }
    }
}
