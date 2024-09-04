using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Manufacturer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        private readonly List<Product> _product = [];
        public IEnumerable<Product> Products => _product;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Manufacturer(Guid id, string name, DateTime createdAt)
        {
            IsNameValid(name);
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }
        public Manufacturer()
        {

        }
        public void ChangeName(string name, DateTime updatedAt)
        {
            IsNameValid(name);
            Name = name;
            UpdatedAt = updatedAt;
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
