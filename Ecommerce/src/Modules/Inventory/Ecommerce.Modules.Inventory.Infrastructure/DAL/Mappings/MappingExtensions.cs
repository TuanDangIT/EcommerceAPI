using Ecommerce.Modules.Inventory.Application.DTO;
using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static ManufacturerDto AsDto(this Manufacturer manufacturer)
            => new ManufacturerDto()
            {
                Name = manufacturer.Name,
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt,
            };
        public static ParameterDto AsDto(this Parameter parameter)
            => new ParameterDto()
            {
                Name = parameter.Name,
                Value = parameter.Value,    
                CreatedAt = parameter.CreatedAt,
                UpdatedAt = parameter.UpdatedAt,
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
