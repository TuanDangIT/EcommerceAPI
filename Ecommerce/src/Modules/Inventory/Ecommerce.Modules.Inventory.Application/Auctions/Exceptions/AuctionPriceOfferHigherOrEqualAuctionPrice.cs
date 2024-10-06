using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Exceptions
{
    internal class AuctionOfferPriceHigherOrEqualAuctionPrice(decimal offerPrice, decimal auctionPrice) : 
        EcommerceException($"Price offer: {offerPrice} cannot be higher or equal auction price: {auctionPrice}.")
    {
    }
}
