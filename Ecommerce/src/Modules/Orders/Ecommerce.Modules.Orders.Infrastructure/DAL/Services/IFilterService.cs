using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Services
{
    public interface IFilterService
    {
        IQueryable<Order> ApplyFilter(IQueryable<Order> query, string propertyPath, string filterValue);
    }
}
