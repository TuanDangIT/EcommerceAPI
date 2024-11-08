using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = [];
        public Role()
        {
            
        }
        public Role(string name)
        {
            Name = name;    
        }
        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
