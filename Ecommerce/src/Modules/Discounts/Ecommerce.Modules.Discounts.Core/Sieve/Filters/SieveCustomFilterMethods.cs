using Ecommerce.Modules.Discounts.Core.Entities;
using Sieve.Services;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;

namespace Ecommerce.Modules.Discounts.Core.Sieve.Filters
{
    internal class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<Offer> Status(IQueryable<Offer> source, string op, string[] values)
        {
            if (values.Length != 1 || !Enum.TryParse(typeof(OfferStatus), values[0], true, out var statusEnum))
            {
                return source;
            }

            var status = (OfferStatus)statusEnum;

            return op switch
            {
                "==" => source.Where(o => o.Status == status),
                "@=" => source.Where(o => EF.Functions.Like(o.Status.ToString(), $"%{values[0]}%")),
                _ => source
            };
        }
        public IQueryable<Coupon> Coupon(IQueryable<Coupon> source, string op, string[] values)
        {
            if (values.Length != 1 || !Enum.TryParse(typeof(CouponType), values[0], true, out var typeEnum))
            {
                return source;
            }

            var type = (CouponType)typeEnum;

            return op switch
            {
                "==" => source.Where(c => c.Type == type),
                "@=" => source.Where(c => EF.Functions.Like(c.Type.ToString(), $"%{values[0]}%")),
                _ => source
            };
        }
    }
}
