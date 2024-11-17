using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.BrowseShipments;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Infrastructure.DAL.Mappings;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.QueryHandlers
{
    internal class BrowseShipmentsHandler : IQueryHandler<BrowseShipments, CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>>
    {
        private readonly OrdersDbContext _dbContext;
        private readonly IFilterService _filterService;

        public BrowseShipmentsHandler(OrdersDbContext dbContext, IFilterService filterService)
        {
            _dbContext = dbContext;
            _filterService = filterService;
        }
        public async Task<CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>> Handle(BrowseShipments request, CancellationToken cancellationToken)
        {
            var shipmentsAsQueryable = _dbContext.Shipments
                .OrderByDescending(s => s.LabelCreatedAt)
                .ThenBy(s => s.Id)
                .Where(s => s.TrackingNumber != null)
                .AsQueryable();
            if (request.Filters is not null && request.Filters.Count != 0)
            {
                foreach (var filter in request.Filters)
                {
                    shipmentsAsQueryable = _filterService.ApplyFilter(shipmentsAsQueryable, filter.Key, filter.Value);
                }
            }
            int takeAmount = request.PageSize + 1;
            if (request.CursorDto is not null)
            {
                if (request.IsNextPage is true)
                {
                    shipmentsAsQueryable = shipmentsAsQueryable.Where(s => s.LabelCreatedAt <= request.CursorDto.CursorLabelCreatedAt && s.Id != request.CursorDto.CursorId);
                }
                else
                {
                    shipmentsAsQueryable = shipmentsAsQueryable.Where(s => s.LabelCreatedAt >= request.CursorDto.CursorLabelCreatedAt && s.Id != request.CursorDto.CursorId);
                    takeAmount = request.PageSize;
                }
            }
            shipmentsAsQueryable = shipmentsAsQueryable.Take(takeAmount);
            if (request.IsNextPage is false && request.CursorDto is not null)
            {
                shipmentsAsQueryable = shipmentsAsQueryable.Reverse();
            }
            var shipments = await shipmentsAsQueryable
                .Select(s => s.AsBrowseDto())
                .AsNoTracking()
                .ToListAsync();
            bool isFirstPage = request.CursorDto is null
                || (request.CursorDto is not null && shipments.First().Id == _dbContext.Shipments.OrderByDescending(s => s.LabelCreatedAt)
                    .ThenBy(s => s.Id).AsNoTracking().First().Id);
            bool hasNextPage = shipments.Count > request.PageSize
                || (request.CursorDto is not null && request.IsNextPage == false);
            if (shipments.Count > request.PageSize)
            {
                shipments.RemoveAt(shipments.Count - 1);
            }
            ShipmentCursorDto nextCursor = hasNextPage ?
                new ShipmentCursorDto()
                {
                    CursorId = shipments.Last().Id,
                    CursorLabelCreatedAt = shipments.Last().CreatedAt
                }
                : new();
            ShipmentCursorDto previousCursor = shipments.Count > 0 ?
                new ShipmentCursorDto()
                {
                    CursorId = shipments.First().Id,
                    CursorLabelCreatedAt = shipments.First().CreatedAt
                }
                : new();
            return new CursorPagedResult<ShipmentBrowseDto, ShipmentCursorDto>(shipments, nextCursor, previousCursor, isFirstPage);
        }
    }
}
