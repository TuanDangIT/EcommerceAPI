using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Order Order { get; set; } = new();
        public Return? Return { get; set; } 
        public Complaint? Complaint { get; set; }
        public Customer(string firstName, string lastName, string email, string phoneNumber, Guid? userId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            UserId = userId ?? Guid.Empty;
        }
        public Customer()
        {

        }
    }
}
