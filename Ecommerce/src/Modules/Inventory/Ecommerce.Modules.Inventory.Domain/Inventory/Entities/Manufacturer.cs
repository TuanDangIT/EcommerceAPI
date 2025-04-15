using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Manufacturer : AggregateRoot, IAuditable
    {
        public string Name { get; private set; } = string.Empty;
        private readonly List<Product> _product = [];
        public IEnumerable<Product> Products => _product;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Manufacturer(string name)
        {
            Validate(name);
            Name = name;
        }
        public Manufacturer()
        {

        }
        public void ChangeName(string name)
        {
            Validate(name);
            Name = name;
            IncrementVersion();
        }
        public static bool IsNameValid(string name)
        {
            if (name is null || name.Length < 2 && name.Length > 32)
            {
                return false;
            }
            return true;
        }
        private static void Validate(string name)
        {
            if (!IsNameValid(name))
            {
                throw new ManufacturerInvalidNameLengthException();
            }
        }
    }
}
