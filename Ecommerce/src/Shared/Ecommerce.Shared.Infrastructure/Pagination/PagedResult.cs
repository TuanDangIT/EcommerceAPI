using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination
{
    //Generyczne DTO do paginacja dla metody w program.cs
    public sealed record class PagedResult<T>
    {
        //Potrzebne informację do przekazania użytkownikowi
        public IEnumerable<T> Items { get; private set; }
        public int CurrentPageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int ItemsFrom { get; private set; }
        public int ItemsTo { get; private set; }
        public int TotalItemsCount { get; private set; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
        {
            //Tutaj mamy przypisanie do konstruktora, aby działało. Potrzebne nam są tu wyliczenia matematyczne, żeby to działało.
            Items = items;
            CurrentPageNumber = pageNumber;
            TotalItemsCount = totalCount;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
