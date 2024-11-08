using Ecommerce.Modules.Mails.Api.DTO;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Events.Externals
{
    public sealed record class ReturnRejected(Guid ReturnId, Guid OrderId, Guid? CustomerId, string FirstName, string Email, string RejectReason, IEnumerable<ProductDto> Products, DateTime CreatedAt) : IEvent;
}
