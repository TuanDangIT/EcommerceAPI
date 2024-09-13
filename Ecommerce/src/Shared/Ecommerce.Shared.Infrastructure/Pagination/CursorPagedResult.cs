using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination
{
    public sealed record class CursorPagedResult<TData, TCursor>
    {
        public IEnumerable<TData> Items { get; private set; }
        public TCursor? NextCursor { get; private set; }
        public TCursor? PreviousCursor { get; private set; }
        public bool IsFirstPage { get; private set; }

        public CursorPagedResult(IEnumerable<TData> items, TCursor? nextCursor,
            TCursor? previousCursor, bool isFirstPage)
        {
            Items = items;
            NextCursor = nextCursor;
            PreviousCursor = previousCursor;
            IsFirstPage = isFirstPage;
        }
    }
}
