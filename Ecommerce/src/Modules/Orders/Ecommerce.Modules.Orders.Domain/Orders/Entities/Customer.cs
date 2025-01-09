using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    //This could be merged with order as one table. It would improve reading performance.
    public class Customer : BaseEntity
    {
        public Guid? UserId { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public Address Address { get; private set; } = new();
        public Order Order { get; private set; } = default!;
        public Guid OrderId { get; private set; }
        public Customer(string firstName, string lastName, string email, string phoneNumber, Address address, Guid? userId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            UserId = userId ?? Guid.Empty;
        }
        public Customer()
        {

        }
    }
}
