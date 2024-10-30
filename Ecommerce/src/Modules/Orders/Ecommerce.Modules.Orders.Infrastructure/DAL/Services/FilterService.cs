using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Services
{
    internal class FilterService : IFilterService
    {
        public IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string filterValue)
        {

            var properties = propertyPath.Split('.');
            if (properties.Length > 1)
            {
                var navigationProperty = properties[0];
                var subPropertyName = properties[1];
                return ApplyNestedFilter(query, propertyPath, navigationProperty, subPropertyName, filterValue);

            }
            else
            {
                var property = typeof(TEntity).GetProperty(propertyPath) ?? throw new InvalidFilterException(propertyPath);
                if (property.PropertyType.IsEnum)
                {
                    return ApplyEnumFilter(query, property.PropertyType, propertyPath, filterValue);
                    //var enumValue = Enum.Parse(property.PropertyType, filterValue, ignoreCase: true);
                    //query = query.Where(p => EF.Property<object>(p, propertyPath).Equals(enumValue));
                    //if (Enum.TryParse(property.PropertyType, filterValue, ignoreCase: true, out var enumValue))
                    //{
                    //    query = query.Where(p => EF.Property<object>(p, propertyPath).Equals(enumValue));
                    //}
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    return ApplyDateFilter(query, propertyPath, filterValue);
                }
                else
                {
                    query = query.Where(x => EF.Property<string>(x!, propertyPath).Contains(filterValue));
                }
            }
            return query;
        }
        private IQueryable<TEntity> ApplyEnumFilter<TEntity>(IQueryable<TEntity> query, Type propertyType, string propertyPath, string filterValue)
        {
            var enumValue = Enum.Parse(propertyType, filterValue, ignoreCase: true);
            query = query.Where(x => EF.Property<object>(x!, propertyPath).Equals(enumValue));
            return query;
        }
        private IQueryable<TEntity> ApplyDateFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string filterValue)
        {
            var dateRange = filterValue.Split("to");
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (dateRange.Length > 0 && DateTime.TryParse(dateRange[0].Trim(), out DateTime parsedFromDate))
            {
                fromDate = parsedFromDate;
            }

            if (dateRange.Length > 1 && DateTime.TryParse(dateRange[1].Trim(), out DateTime parsedToDate))
            {
                toDate = parsedToDate;
            }
            if (fromDate is not null && toDate is not null)
            {
                return query.Where(x => EF.Property<DateTime>(x!, propertyPath) >= fromDate.Value &&
                                        EF.Property<DateTime>(x!, propertyPath) <= toDate.Value);
            }
            else if (fromDate is not null)
            {
                return query.Where(x => EF.Property<DateTime>(x!, propertyPath) >= fromDate.Value);
            }
            else if (toDate is not null)
            {
                return query.Where(x => EF.Property<DateTime>(x!, propertyPath) <= toDate.Value);
            }
            return query;
        }
        private IQueryable<TEntity> ApplyNestedFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string navigationProperty, string subPropertyName, string filterValue)
        {
            var navigation = typeof(TEntity).GetProperty(navigationProperty) ?? throw new InvalidFilterException(propertyPath);
            var subProperty = navigation.PropertyType.GetProperty(subPropertyName) ?? throw new InvalidFilterException(propertyPath);
            if (navigation == null || subProperty == null)
            {
                throw new InvalidFilterException(propertyPath);
            }
            if (subProperty.PropertyType.IsEnum)
            {
                //var enumValue = Enum.Parse(subProperty.PropertyType, filterValue, ignoreCase: true);
                //query = query.Where(p => EF.Property<object>(EF.Property<object>(p, navigationProperty), subPropertyName)
                //    .Equals(enumValue));
                return ApplyEnumFilter(query, subProperty.PropertyType, propertyPath, filterValue);
            }
            else
            {
                query = query.Where(x => EF.Property<string>(EF.Property<object>(x!, navigationProperty), subPropertyName)
                    .Contains(filterValue));
            }
            return query;
        }
    }
}
