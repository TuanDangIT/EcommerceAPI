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
        public static ManufacturerBrowseDto AsDto(this Manufacturer manufacturer)
            => new ManufacturerBrowseDto()
            {
                Name = manufacturer.Name,
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt,
            };
        public static ParameterBrowseDto AsDto(this Domain.Entities.Parameter parameter)
            => new ParameterBrowseDto()
            {
                Name = parameter.Name, 
                CreatedAt = parameter.CreatedAt,
                UpdatedAt = parameter.UpdatedAt,
            };
        public static CategoryBrowseDto AsDto(this Category category)
            => new CategoryBrowseDto()
            {
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
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
