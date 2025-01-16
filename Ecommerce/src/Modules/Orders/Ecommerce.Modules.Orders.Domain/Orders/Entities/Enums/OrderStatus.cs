using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums
{
    public enum OrderStatus
    {
        Placed,
        ParcelPacked,
        Shipped,
        Completed,
        Cancelled,
        Returned,
        ReturnRejected,
        Complicated
    }
}
