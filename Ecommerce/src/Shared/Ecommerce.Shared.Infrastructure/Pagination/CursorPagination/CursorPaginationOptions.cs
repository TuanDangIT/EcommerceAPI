using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination
{
    public class CursorPaginationOptions
    {
        public int DefaultPageSize { get; set; }
        public int MaxPageSize { get; set; }
    }
}
