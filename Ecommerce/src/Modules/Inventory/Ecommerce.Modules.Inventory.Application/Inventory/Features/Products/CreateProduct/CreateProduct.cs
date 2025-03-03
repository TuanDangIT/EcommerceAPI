using Ecommerce.Modules.Inventory.Application.Inventory.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.CreateProduct
{
    public sealed record class CreateProduct : ICommand<Guid>
    {
        public string SKU { get; set; } = string.Empty;
        public string? EAN { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int VAT { get; set; }
        public int? Quantity { get; set; }
        public string? Location { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? AdditionalDescription { get; set; }
        public List<ProductParameterDto>? ProductParameters { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid CategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    };
}
