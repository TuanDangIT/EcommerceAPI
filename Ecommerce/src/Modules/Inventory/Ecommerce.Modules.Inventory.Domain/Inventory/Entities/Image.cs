using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Image : BaseEntity
    {
        public string ImageUrlPath { get; private set; } = string.Empty;
        public int Order { get; private set; }
        public Product Product { get; private set; } = new();
        public Guid ProductId { get; private set; }
        public Image(Guid id, string imageUrlPath, int order)
        {
            if(order >= 8)
            {
                throw new ImageOrderOutOfBoundException();
            }
            Id = id;
            ImageUrlPath = imageUrlPath;
            Order = order;
        }
        public Image(Guid id, string imageUrlPath, int order, Product product)
        {
            if (order >= 8)
            {
                throw new ImageOrderOutOfBoundException();
            }
            Id = id;
            ImageUrlPath = imageUrlPath;
            Order = order;
            Product = product;
        }
    }
}
