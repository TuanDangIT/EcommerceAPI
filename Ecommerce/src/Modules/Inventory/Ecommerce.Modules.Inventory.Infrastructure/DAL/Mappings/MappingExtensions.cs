using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }
        public static ProductListingDto AsListingDto(this Product product)
        {
            throw new NotImplementedException();
        }
    }
}
