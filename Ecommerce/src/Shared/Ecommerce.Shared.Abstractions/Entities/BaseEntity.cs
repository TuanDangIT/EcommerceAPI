using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; protected set; } = default!;
    }
    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}
