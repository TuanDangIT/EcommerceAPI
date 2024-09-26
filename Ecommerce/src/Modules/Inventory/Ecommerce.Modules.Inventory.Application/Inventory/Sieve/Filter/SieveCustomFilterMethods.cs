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
        public IQueryable<Product> ParameterNameAndValue(IQueryable<Product> source, string op, string[] values)
        {
            //if (op == "@=")
            //{
            //    return source.Where(p => p.Parameters.Any(p => values.Any(v => p.Name.Contains(v))));
            //}
            //else if (op == "==")
            //{
            //    return source.Where(p => p.Parameters.Any(p => values.Any(v => v == p.Name)));
            //}
            //return source;
            //if (op == "@=")
            //{
            //    return source.Where(p => p.Parameters.Any(p => p.Name.Contains(values[0])));
            //}
            //else if (op == "==")
            //{
            //    return source.Where(p => p.Parameters.Any(p => values[0] == p.Name));
            //}

            switch ((op, values.Length))
            {
                case ("==", 1):
                    return source.Where(p => p.Parameters.Any(p => values.Any(v => v == p.Name)));
                case ("==", 2):
                    return source.Where(p => p.Parameters.Any(pa => values[0] == pa.Name && p.ProductParameters.Single(pp => pp.ParameterId == pa.Id).Value == values[1]));
                case ("@=", 1):
                    return source.Where(p => p.Parameters.Any(p => values.Any(v => p.Name.Contains(v))));
                case ("@=", 2):
                    return source.Where(p => p.Parameters.Any(pa => pa.Name.Contains(values[0]) && p.ProductParameters.Single(pp => pp.ParameterId == pa.Id).Value.Contains(values[1])));
            }
            return source;
        }
        //public IQueryable<Product> ProductParameterValue(IQueryable<Product> source, string op, string[] values)
        //{

        //}
    }
}
