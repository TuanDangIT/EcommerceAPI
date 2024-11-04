using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseOrdersHandler : IQueryHandler<BrowseOrders, CursorPagedResult<OrderBrowseDto, OrderCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseOrdersHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<OrderBrowseDto, OrderCursorDto>> Handle(BrowseOrders request, CancellationToken cancellationToken)
        {
            var ordersAsQueryable = _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Shipment)
                .Include(o => o.Invoice)
                .OrderByDescending(o => o.OrderPlacedAt)
                .ThenBy(o => o.Id)
                .AsQueryable();
            if (request.Filters is not null && request.Filters.Count != 0)
            {
                foreach (var filter in request.Filters)
                {
                    ordersAsQueryable = _filterService.ApplyFilter(ordersAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if(request.CursorDto is not null)
            {
                if(request.IsNextPage is true)
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.OrderPlacedAt <= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                }
                else
                {
                    ordersAsQueryable = ordersAsQueryable.Where(o => o.OrderPlacedAt >= request.CursorDto.CursorOrderPlacedAt && o.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            ordersAsQueryable = ordersAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                ordersAsQueryable = ordersAsQueryable.Reverse();
            }
            var orders = await ordersAsQueryable
                //.Include(o => o.Products)
                .Select(o => o.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && orders.First().Id == _dbContext.Orders.OrderByDescending(o => o.Id).AsNoTracking().First().Id);
            bool hasNextPage = orders.Count > request.PageSize 
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (orders.Count > request.PageSize)
            {
                orders.RemoveAt(orders.Count - 1);
            }
            OrderCursorDto nextCursor = hasNextPage ?
                new OrderCursorDto()
                {
                    CursorId = orders.Last().Id,
                    CursorOrderPlacedAt = orders.Last().OrderPlacedAt
                }
                : new();
            OrderCursorDto previousCursor = orders.Count > 0 ?
                new OrderCursorDto()
                {
                    CursorId = orders.First().Id,
                    CursorOrderPlacedAt = orders.First().OrderPlacedAt
                }
                : new();
            return new CursorPagedResult<OrderBrowseDto, OrderCursorDto>(orders, nextCursor, previousCursor, isFirstPage);
        }
        //private IQueryable<Order> ApplyFilter(IQueryable<Order> query, string propertyPath, string filterValue)
        //{
            
        //    var properties = propertyPath.Split('.');
        //    if (properties.Length > 1)
        //    {
        //        var navigationProperty = properties[0];
        //        var subPropertyName = properties[1];
                
        //    }
        //    else
        //    {
        //        var property = typeof(Order).GetProperty(propertyPath) ?? throw new OrderWrongFilterException(propertyPath);
        //        if (property.PropertyType.IsEnum)
        //        {
        //            return ApplyEnumFilter(query, property.PropertyType, propertyPath, filterValue);
        //            //var enumValue = Enum.Parse(property.PropertyType, filterValue, ignoreCase: true);
        //            //query = query.Where(p => EF.Property<object>(p, propertyPath).Equals(enumValue));
        //            //if (Enum.TryParse(property.PropertyType, filterValue, ignoreCase: true, out var enumValue))
        //            //{
        //            //    query = query.Where(p => EF.Property<object>(p, propertyPath).Equals(enumValue));
        //            //}
        //        }
        //        else if (property.PropertyType == typeof(DateTime))
        //        {
        //            return ApplyDateFilter(query, propertyPath, filterValue);
        //        }
        //        else
        //        {
        //            query = query.Where(o => EF.Property<string>(o, propertyPath).Contains(filterValue));
        //        }
        //    }
        //    return query;
        //}
        //private IQueryable<Order> ApplyEnumFilter(IQueryable<Order> query, Type propertyType, string propertyPath, string filterValue)
        //{
        //    var enumValue = Enum.Parse(propertyType, filterValue, ignoreCase: true);
        //    query = query.Where(p => EF.Property<object>(p, propertyPath).Equals(enumValue));
        //    return query;
        //}
        //private IQueryable<Order> ApplyDateFilter(IQueryable<Order> query, string propertyPath, string filterValue)
        //{
        //    var dateRange = filterValue.Split("to");
        //    DateTime? fromDate = null;
        //    DateTime? toDate = null;

        //    if (dateRange.Length > 0 && DateTime.TryParse(dateRange[0].Trim(), out DateTime parsedFromDate))
        //    {
        //        fromDate = parsedFromDate;
        //    }

        //    if (dateRange.Length > 1 && DateTime.TryParse(dateRange[1].Trim(), out DateTime parsedToDate))
        //    {
        //        toDate = parsedToDate;
        //    }
        //    if (fromDate is not null && toDate is not null)
        //    {
        //        return query.Where(p => EF.Property<DateTime>(p, propertyPath) >= fromDate.Value &&
        //                                EF.Property<DateTime>(p, propertyPath) <= toDate.Value);
        //    }
        //    else if (fromDate is not null)
        //    {
        //        return query.Where(p => EF.Property<DateTime>(p, propertyPath) >= fromDate.Value);
        //    }
        //    else if (toDate is not null)
        //    {
        //        return query.Where(p => EF.Property<DateTime>(p, propertyPath) <= toDate.Value);
        //    }
        //    return query;
        //}
        //private IQueryable<Order> ApplyNestedFilter(IQueryable<Order> query, string propertyPath, string navigationProperty, string subPropertyName, string filterValue)
        //{
        //    var navigation = typeof(Order).GetProperty(navigationProperty);
        //    var subProperty = navigation?.PropertyType.GetProperty(subPropertyName);
        //    if (navigation == null || subProperty == null)
        //    {
        //        throw new OrderWrongFilterException(propertyPath);
        //    }
        //    if (subProperty.PropertyType.IsEnum)
        //    {
        //        //var enumValue = Enum.Parse(subProperty.PropertyType, filterValue, ignoreCase: true);
        //        //query = query.Where(p => EF.Property<object>(EF.Property<object>(p, navigationProperty), subPropertyName)
        //        //    .Equals(enumValue));
        //        return ApplyEnumFilter(query, subProperty.PropertyType, propertyPath, filterValue);
        //    }
        //    else
        //    {
        //        query = query.Where(p => EF.Property<string>(EF.Property<object>(p, navigationProperty), subPropertyName)
        //            .Contains(filterValue));
        //    }
        //    return query;
        //}
    }
}
