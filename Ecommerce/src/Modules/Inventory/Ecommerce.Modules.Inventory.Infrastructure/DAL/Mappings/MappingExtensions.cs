using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static ManufacturerBrowseDto AsBrowseDto(this Manufacturer manufacturer)
            => new ManufacturerBrowseDto()
            {
                Name = manufacturer.Name,
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt,
            };
        public static ParameterBrowseDto AsBrowseDto(this Domain.Inventory.Entities.Parameter parameter)
            => new ParameterBrowseDto()
            {
                Name = parameter.Name, 
                CreatedAt = parameter.CreatedAt,
                UpdatedAt = parameter.UpdatedAt,
            };
        public static CategoryBrowseDto AsBrowseDto(this Category category)
            => new CategoryBrowseDto()
            {
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
            };
        public static ManufacturerOptionDto AsOptionDto(this Manufacturer manufacturer)
            => new ManufacturerOptionDto()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
            };
        public static ParameterOptionDto AsOptionDto(this Domain.Inventory.Entities.Parameter parameter)
            => new ParameterOptionDto()
            {
                Id = parameter.Id,
                Name = parameter.Name,
            };
        public static CategoryOptionDto AsOptionDto(this Category category)
            => new CategoryOptionDto()
            {
                Id = category.Id,
                Name = category.Name,
            };
        public static ProductDetailsDto AsDetailsDto(this Product product)
        {
            var productDetailsImagesDto = new List<ProductDetailsImageDto>();
            foreach(var image in product.Images) 
            {
                productDetailsImagesDto.Add(new ProductDetailsImageDto()
                {
                    ImageUrlPath = image.ImageUrlPath,
                    Order = image.Order,
                });
            }
            var productDetailsParametersDto = new List<ProductDetailsParameterDto>();
            foreach(var productParameter in product.ProductParameters)
            {
                productDetailsParametersDto.Add(new ProductDetailsParameterDto()
                {
                    Parameter = productParameter.Parameter.Name,
                    Value = productParameter.Value
                });
            }
            var productDetailsDto =  new ProductDetailsDto()
            {
                SKU = product.SKU,
                EAN = product.EAN,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                VAT = product.VAT,
                Location = product.Location,
                Description = product.Description,
                AdditionalDescription = product.AdditionalDescription,
                Manufacturer = product.Manufacturer.Name,
                Category = product.Category.Name,
                Images = productDetailsImagesDto,
                Parameters = productDetailsParametersDto
            };
            return productDetailsDto;
        }
        public static ProductBrowseDto AsListingDto(this Product product)
            => new ProductBrowseDto()
            {
                SKU = product.SKU,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
            };
        public static AuctionBrowseDto AsBrowseDto(this Auction auction)
            => new AuctionBrowseDto()
            {
                Name = auction.Name,
                Price = auction.Price,
                Quantity = auction.Quantity,
                ImageUrl = auction.ImagePathUrls[0],
                Category = auction.Category,
                AverageGrade = auction.Reviews.Any() ? auction.Reviews.Average(p => p.Grade) : 0
            };
        public static AuctionDetailsDto AsDetailsDto(this Auction auction)
        {
            var reviews = new List<ReviewDto>();
            
            return new AuctionDetailsDto()
            {
                SKU = auction.SKU,
                Name = auction.Name,
                Price = auction.Price,
                Quantity = auction.Quantity,
                Description = auction.Description,
                AdditionalDescription = auction.AdditionalDescription,
                Parameters = auction.Parameters,
                Manufacturer = auction.Manufacturer,
                ImageUrls = auction.ImagePathUrls,
                Category = auction.Category,
                Reviews = auction.Reviews.Select(r => r.AsDto()).ToList(),
                AverageGrade = auction.Reviews.Any() ? auction.Reviews.Average(p => p.Grade) : 0
            };
        }
        public static ReviewDto AsDto(this Review review)
            => new ReviewDto()
            {
                Id = review.Id,
                Username = review.Username,
                Text = review.Text,
                Grade = review.Grade,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            };
    }
}
