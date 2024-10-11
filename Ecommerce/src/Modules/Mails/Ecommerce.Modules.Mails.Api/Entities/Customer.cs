using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Entities
{
    internal class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        private readonly List<Mail> _mails = [];
        public IEnumerable<Mail> Mails => _mails;
        public Customer(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
        public Customer()
        {
            
        }
    }
}
