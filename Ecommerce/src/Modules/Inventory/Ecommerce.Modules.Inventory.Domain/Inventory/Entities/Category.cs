using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Category : AggregateRoot, IAuditable
    {
        public string Name { get; private set; } = string.Empty;
        private readonly List<Product> _product = [];
        public IEnumerable<Product> Products => _product;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Category(Guid id, string name)
        {
            IsNameValid(name);
            Id = id;
        }
        public Category()
        {

        }
        public void ChangeName(string name)
        {
            IsNameValid(name);
            Name = name;
            IncrementVersion();
        }
        private static void IsNameValid(string name)
        {
            if (name.Length >= 2 && name.Length <= 32)
            {
                throw new CategoryInvalidNameLengthException();
            }
        }
    }
}
