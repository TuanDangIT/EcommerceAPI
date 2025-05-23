﻿using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.GetAuction
{
    public sealed record class GetAuction(Guid AuctionId) : IQuery<AuctionDetailsDto?>;
}
