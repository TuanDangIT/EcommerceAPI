using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services
{
    internal class FilterService : IFilterService
    {
        private readonly string[] _dateSeparators = ["-to-", "to-", "-to"];
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
                var property = typeof(TEntity).GetProperty(propertyPath, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidFilterException(propertyPath);
                return property.PropertyType switch
                {
                    Type t when t.IsEnum => ApplyEnumFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(DateTime) => ApplyDateFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(string) => ApplyStringFilter(query, property, null, filterValue),
                    Type t when t == typeof(decimal) => ApplyDecimalFilter(query, propertyPath, property, null, filterValue),
                    Type t when t == typeof(int) => ApplyIntegerFilter(query, propertyPath, property, null, filterValue),
                    _ => throw new InvalidFilterException(propertyPath)
                };
            }
        }
        private IQueryable<TEntity> ApplyNestedFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, string navigationProperty, string subPropertyName, string filterValue)
        {
            var navigation = typeof(TEntity).GetProperty(navigationProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidFilterException(propertyPath);
            var subProperty = navigation.PropertyType.GetProperty(subPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidFilterException(propertyPath);
            if (navigation == null || subProperty == null)
            {
                throw new InvalidFilterException(propertyPath);
            }
            return subProperty.PropertyType switch
            {
                Type t when t.IsEnum => ApplyEnumFilter(query, propertyPath, navigation, subProperty, filterValue),
                Type t when t == typeof(DateTime) => ApplyDateFilter(query, propertyPath, navigation, subProperty, filterValue),
                Type t when t == typeof(string) => ApplyStringFilter(query, navigation, subProperty, filterValue),
                Type t when t == typeof(decimal) => ApplyDecimalFilter(query, propertyPath, navigation, subProperty, filterValue),
                Type t when t == typeof(int) => ApplyIntegerFilter(query, propertyPath, navigation, subProperty, filterValue),
                _ => throw new InvalidFilterException(propertyPath)
            };
        }
        private IQueryable<TEntity> ApplyEnumFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!Enum.TryParse(property.PropertyType, filterValue, ignoreCase: true, out var enumValue))
            {
                throw new InvalidFilterException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<object>(x!, property.Name).Equals(enumValue))
                : query = query.Where(x => EF.Property<object>(EF.Property<object>(x!, subProperty.Name), property.Name).Equals(enumValue));
        }
        private IQueryable<TEntity> ApplyStringFilter<TEntity>(IQueryable<TEntity> query, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
            => subProperty is null
                ? query.Where(x => EF.Property<string>(x!, property.Name).ToLower().Contains(filterValue.ToLower()))
                : query.Where(x => EF.Property<string>(EF.Property<object>(x!, property.Name), subProperty.Name).ToLower().Contains(filterValue.ToLower()));
        private IQueryable<TEntity> ApplyDecimalFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!decimal.TryParse(filterValue, out var parsedDecimal))
            {
                throw new InvalidFilterException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<decimal>(x!, property.Name) == parsedDecimal)
                : query.Where(x => EF.Property<decimal>(EF.Property<object>(x!, property.Name), subProperty.Name) == parsedDecimal);
        }
        private IQueryable<TEntity> ApplyIntegerFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            if (!int.TryParse(filterValue, out var parsedDecimal))
            {
                throw new InvalidFilterException(propertyPath);
            }
            return subProperty is null
                ? query.Where(x => EF.Property<int>(x!, property.Name) == parsedDecimal)
                : query.Where(x => EF.Property<int>(EF.Property<object>(x!, property.Name), subProperty.Name) == parsedDecimal);
        }
        private IQueryable<TEntity> ApplyDateFilter<TEntity>(IQueryable<TEntity> query, string propertyPath, PropertyInfo property, PropertyInfo? subProperty, string filterValue)
        {
            var dateRange = filterValue.Split(_dateSeparators, StringSplitOptions.None);
            var fromDate = ParseDateOrThrow(dateRange[0], propertyPath)?.Date;
            var toDate = ParseDateOrThrow(dateRange[1], propertyPath)?.Date.AddDays(1).AddTicks(-1);
            return subProperty is null
                ? query.Where(x =>
                    (!fromDate.HasValue || EF.Property<DateTime>(x!, property.Name) >= fromDate.Value.ToUniversalTime()) &&
                    (!toDate.HasValue || EF.Property<DateTime>(x!, property.Name) <= toDate.Value.ToUniversalTime()))
                : query.Where(x =>
                    (!fromDate.HasValue || EF.Property<DateTime>(EF.Property<object>(x!, subProperty.Name), property.Name) >= fromDate.Value.ToUniversalTime()) &&
                    (!toDate.HasValue || EF.Property<DateTime>(EF.Property<object>(x!, subProperty.Name), property.Name) <= toDate.Value.ToUniversalTime()));
        }
        private DateTime? ParseDateOrThrow(string? dateString, string propertyPath)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }
            return DateTime.TryParse(dateString.Trim(), out var parsedDate)
                ? parsedDate
                : throw new InvalidFilterException(propertyPath);
        }
    }
}
