using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Sieve.Filters
{
    internal class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<Product> ProductParameterNameAndValue(IQueryable<Product> source, string op, string[] values)
        {
            //This filter is for first when values length is equal to 0 then it filters only by name, if 2 then by name and value associated with that parameter name.
            switch ((op, values.Length))
            {
                case ("==", 1):
                    return source.Where(p => p.Parameters.Any(p => values.Any(v => v.ToLower() == p.Name.ToLower())));
                case ("==", 2):
                    return source.Where(p => p.Parameters.Any(pa => values[0].ToLower() == pa.Name.ToLower() && p.ProductParameters.First(pp => pp.ParameterId == pa.Id).Value.ToLower() == values[1].ToLower()));
                case ("@=", 1):
                    return source.Where(p => p.Parameters.Any(p => values.Any(v => p.Name.ToLower().Contains(v.ToLower()))));
                case ("@=", 2):
                    return source.Where(p => p.Parameters.Any(pa => pa.Name.ToLower().Contains(values[0].ToLower()) && p.ProductParameters.First(pp => pp.ParameterId == pa.Id).Value.ToLower().Contains(values[1].ToLower())));
                default:
                    break;
            }
            return source;
        }
        public IQueryable<Auction> AuctionParameterNameAndValue(IQueryable<Auction> source, string op, string[] values)
        {
            switch ((op, values.Length))
            {
                case ("==", 1):
                    return source.Where(a => a.Parameters != null && a.Parameters.Any(p => values.Any(v => v.ToLower() == p.Name.ToLower())));
                case ("==", 2):
                    return source.Where(a => a.Parameters != null && a.Parameters.Any(p => p.Name.ToLower() == values[0].ToLower() && p.Value.ToLower() == values[1].ToLower()));
                case ("@=", 1):
                    return source.Where(a => a.Parameters != null && a.Parameters.Any(p => values.Any(v => p.Name.ToLower().Contains(v.ToLower()))));
                case ("@=", 2):
                    return source.Where(a => a.Parameters != null && a.Parameters.Any(p => p.Name.ToLower().Contains(values[0].ToLower()) && p.Value.ToLower().Contains(values[1].ToLower())));
                default:
                    break;
            }
            return source;
        }
    }
}
