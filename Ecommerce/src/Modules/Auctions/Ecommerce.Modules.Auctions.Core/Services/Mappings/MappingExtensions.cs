using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Services.Mappings
{
    internal static class MappingExtensions
    {
        public static AuctionBrowseDto AsBrowseDto(this Auction auction)
            => new AuctionBrowseDto()
            {
                Name = auction.Name,
                Price = auction.Price,
                Quantity = auction.Quantity,
                ImageUrl = auction.ImagePathUrls[0],
                Category = auction.Category,
                AverageGrade = auction.Reviews is not null ? auction.Reviews.Average(p => p.Grade) : 0
            };
        public static AuctionDetailsDto AsDetailsDto(this Auction auction)
            => new AuctionDetailsDto()
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
                Reviews = auction.Reviews,
                AverageGrade = auction.Reviews is not null ? auction.Reviews.Average(p => p.Grade) : 0
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
