using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL.Mappings
{
    public static class MappingExtensions
    {
        public static NominalDiscountBrowseDto AsNominalDto(this NominalDiscount discount)
            => new NominalDiscountBrowseDto()
            {
                Code = discount.Code,
                //Type = discount.Type.ToString(),
                NominalValue = discount.NominalValue,
                EndingDate = discount.EndingDate,
                CreatedAt = discount.CreatedAt,
            };
        public static PercentageDiscountBrowseDto AsPercentageDto(this PercentageDiscount discount)
            => new PercentageDiscountBrowseDto()
            {
                Code = discount.Code,
                //Type = discount.Type.ToString(),
                Percent = discount.Percent,
                EndingDate = discount.EndingDate,
                CreatedAt = discount.CreatedAt,
            };
    }
}
