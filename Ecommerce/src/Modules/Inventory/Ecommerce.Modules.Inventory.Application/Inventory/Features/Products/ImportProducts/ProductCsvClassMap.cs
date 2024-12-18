using CsvHelper.Configuration;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts
{
    internal class ProductCsvClassMap : ClassMap<ProductCsvRecordDto>
    {
        public ProductCsvClassMap(char valueSeperator)
        {
            Map(p => p.SKU);
            Map(p => p.EAN);
            Map(p => p.Name);
            Map(p => p.Price);
            Map(p => p.VAT);
            Map(p => p.Quantity);
            Map(p => p.Location);
            Map(p => p.Description);
            Map(p => p.AdditionalDescription);
            Map(p => p.Manufacturer);
            Map(p => p.Category);
            Map(p => p.Images).Convert(args =>
                args.Row.GetField(nameof(ProductCsvRecordDto.Images))?.Split(valueSeperator).Select(url => url.Trim()).ToList() ??
                throw new ProductCannotHaveNoImagesException()
            );
            Map(p => p.Parameters).Convert(args =>
            {
                var parameters = new Dictionary<string, string>();
                var row = args.Row;
                var headers = row.HeaderRecord ?? throw new NullReferenceException();
                foreach(var header in headers)
                {
                    if (!typeof(ProductCsvRecordDto).GetProperties().Select(p => p.Name).Contains(header))
                    {
                        var parameterValue = row.GetField(header);
                        if (parameterValue is null || parameterValue == "")
                        {
                            continue;
                        }
                        parameters.Add(header, parameterValue);
                    }
                }
                return parameters;
            });
        }
    }
}
