using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.ValueObjects
{
    public class Customer
    {
        public Guid? CustomerId { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public Customer(string firstName, string lastName, string email, string phoneNumber, Guid? customerId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            CustomerId = customerId ?? Guid.Empty;
        }
        public Customer()
        {

        }
        public void SetCustomerId(Guid customerId)
            => CustomerId = customerId;
        public void SetCustomerDetails(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
