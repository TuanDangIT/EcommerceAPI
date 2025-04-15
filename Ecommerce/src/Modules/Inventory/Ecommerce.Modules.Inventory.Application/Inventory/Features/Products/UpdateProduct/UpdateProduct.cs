using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.UpdateProduct
{
    public sealed record class UpdateProduct : ICommand
    {
        [SwaggerIgnore]
        public Guid Id { get; init; }
        public string SKU { get; init; } = string.Empty;
        public string? EAN { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int VAT { get; init; }
        public int? Quantity { get; init; }
        public int? Reserved { get; init; } 
        public string? Location { get; init; }
        public string Description { get; init; } = string.Empty;
        public string? AdditionalDescription { get; init; }
        public List<ProductParameterDto>? ProductParameters { get; init; }
        public Guid? ManufacturerId { get; init; }
        public Guid? CategoryId { get; init; }
        public List<IFormFile> Images { get; init; } = [];
    }
}
