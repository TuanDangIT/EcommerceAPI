using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.EditCustomer
{
    public sealed record class EditCustomer(Guid OrderId, string FirstName, string LastName, string Email, string PhoneNumber, AddressDto Address) : ICommand;
}
