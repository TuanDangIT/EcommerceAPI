using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts
{
    internal class ProductCsvClassMap : ClassMap<ProductCsvRecordDto>
    {
        private readonly char _seperationChar = ',';
        public ProductCsvClassMap(/*char valueSeperator*/)
        {
            Map(p => p.SKU).Validate(args => !string.IsNullOrEmpty(args.Field) && Product.IsSkuValid(args.Field));
            Map(p => p.EAN).Validate(args => Product.IsEanValid(args.Field)).TypeConverterOption.NullValues(string.Empty);
            Map(p => p.Name).Validate(args => !string.IsNullOrEmpty(args.Field) && Product.IsNameValid(args.Field));
            Map(p => p.Price).Validate(args =>
            {
                if (string.IsNullOrEmpty(args.Field))
                    return false;
                return decimal.TryParse(args.Field, out var price)
                       && Product.IsPriceValid(price);
            });
            Map(p => p.VAT).Validate(args => string.IsNullOrEmpty(args.Field) || Product.IsVatValid(int.Parse(args.Field)));
            Map(p => p.Quantity).Validate(args => string.IsNullOrEmpty(args.Field) || Product.IsQuantityValid(int.Parse(args.Field)));
            Map(p => p.Location).Validate(args => Product.IsLocationValid(args.Field)).TypeConverterOption.NullValues(string.Empty);
            Map(p => p.Description).Validate(args => !string.IsNullOrEmpty(args.Field));
            Map(p => p.AdditionalDescription).TypeConverterOption.NullValues("");
            Map(p => p.Manufacturer).Validate(args => string.IsNullOrEmpty(args.Field) || Manufacturer.IsNameValid(args.Field)).TypeConverterOption.NullValues(string.Empty);
            Map(p => p.Category).Validate(args => string.IsNullOrEmpty(args.Field) || Category.IsNameValid(args.Field)).TypeConverterOption.NullValues(string.Empty);
            Map(p => p.Images).Convert(args =>
            {
                if (string.IsNullOrEmpty(args.Row.GetField(nameof(ProductCsvRecordDto.Images))))
                {
                    return [];
                }
                return args.Row.GetField(nameof(ProductCsvRecordDto.Images))!.Split(_seperationChar).Select(url => url.Trim()).ToList();
            });
            Map(p => p.Parameters).Convert(args =>
            {
                var parameters = new Dictionary<string, string>();
                var row = args.Row;
                var headers = row.HeaderRecord ?? throw new NullReferenceException();
                foreach (var header in headers)
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
