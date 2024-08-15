using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        public static ParameterBrowseDto AsBrowseDto(this Domain.Entities.Parameter parameter)
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
        public static ParameterOptionDto AsOptionDto(this Domain.Entities.Parameter parameter)
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
            productDetailsImagesDto.Sort();
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
        public static ProductListingDto AsListingDto(this Product product)
        {
            throw new NotImplementedException();
        }
    }
}
