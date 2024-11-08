using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Entities
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
