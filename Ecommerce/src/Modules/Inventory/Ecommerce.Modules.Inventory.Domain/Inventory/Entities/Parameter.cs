using Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Parameter
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        private readonly List<Product> _products = [];
        public IEnumerable<Product>? Products => _products;
        private readonly List<ProductParameter>? _productParameters = [];
        public IEnumerable<ProductParameter>? ProductParameters => _productParameters;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Parameter(Guid id, string name, DateTime createdAt)
        {
            IsNameValid(name);
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }
        public Parameter()
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
            if(name.Length >= 2 && name.Length <= 32)
            {
                throw new CategoryInvalidNameLengthException();
            }
        }
    }
}
