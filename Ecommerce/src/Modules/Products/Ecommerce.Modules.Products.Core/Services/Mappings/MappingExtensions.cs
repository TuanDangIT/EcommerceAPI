using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Services.Mappings
{
    internal static class MappingExtensions
    {
        public static ProductBrowseDto AsBrowseDto(this Product product)
            => new ProductBrowseDto()
            {
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageUrl = product.ImagePathUrls[0],
                Category = product.Category,
                AverageGrade = product.Reviews is not null ? product.Reviews.Average(p => p.Grade) : 0
            };
        public static ProductDetailsDto AsDetailsDto(this Product product)
            => new ProductDetailsDto()
            {
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                AdditionalDescription = product.AdditionalDescription,
                Parameters = product.Parameters,
                Manufacturer = product.Manufacturer,
                ImageUrls = product.ImagePathUrls,
                Category = product.Category,
                Reviews = product.Reviews,
                AverageGrade = product.Reviews is not null ? product.Reviews.Average(p => p.Grade) : 0
            };
        public static ReviewBrowseDto AsBrowseDto(this Review review)
            => new ReviewBrowseDto()
            {
                Username = review.Username,
                Text = review.Text,
                Grade = review.Grade,
            };
    }
}
