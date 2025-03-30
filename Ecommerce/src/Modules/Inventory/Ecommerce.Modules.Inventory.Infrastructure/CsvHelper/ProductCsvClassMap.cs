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
            Map(p => p.SKU).Validate(args => !string.IsNullOrEmpty(args.Field) && Product.IsSkuValid(args.Field));
            Map(p => p.EAN).Validate(args => Product.IsEanValid(args.Field));
            Map(p => p.Name).Validate(args => !string.IsNullOrEmpty(args.Field) && Product.IsNameValid(args.Field));
            Map(p => p.Price).Validate(args => !string.IsNullOrEmpty(args.Field) && Product.IsPriceValid(decimal.Parse(args.Field)));
            Map(p => p.VAT).Validate(args => string.IsNullOrEmpty(args.Field) || Product.IsVatValid(int.Parse(args.Field)));
            Map(p => p.Quantity).Validate(args => string.IsNullOrEmpty(args.Field) || Product.IsQuantityValid(int.Parse(args.Field)));
            Map(p => p.Location).Validate(args => Product.IsLocationValid(args.Field));
            Map(p => p.Description).Validate(args => !string.IsNullOrEmpty(args.Field));
            Map(p => p.AdditionalDescription);
            Map(p => p.Manufacturer).Validate(args => args.Field.Length <= 32);
            Map(p => p.Category).Validate(args => args.Field.Length <= 32);
            Map(p => p.Images).Convert(args =>
                args.Row.GetField(nameof(ProductCsvRecordDto.Images))?.Split(valueSeperator).Select(url => url.Trim()).ToList() ?? []
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
                        if (string.IsNullOrEmpty(parameterValue))
                        {
                            continue;
                        }
                        parameters.Add(header, parameterValue);
                    }
                }
                return parameters;
            }).Validate(args => args.Row.HeaderRecord?.Length <= 32);
        }
    }
}
