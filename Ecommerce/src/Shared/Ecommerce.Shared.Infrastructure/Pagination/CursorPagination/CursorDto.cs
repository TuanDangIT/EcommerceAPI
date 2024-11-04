using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination
{
    public class CursorDto<T>
    {
        public T? CursorId { get; set; }
    }
}
