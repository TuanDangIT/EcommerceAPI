using Ecommerce.Modules.Auctions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.DAL
{
    internal interface IAuctionDbContext
    {
        DbSet<Auction> Auctions { get; set; }
        DbSet<Review> Reviews { get; set; }
        Task<int> SaveChangesAsync();
    }
}
