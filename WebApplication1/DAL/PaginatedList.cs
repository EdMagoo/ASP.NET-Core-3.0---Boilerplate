using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.DAL
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        /**
         * Constructor to add or create a PaginatedList<T>
         * -params-
         * items: items to append
         * count: number of elements returned by a query
         * pageIndex: index page
         * pageSize: number of elements per page
         */
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            // set the index to point to a page in the pages
            PageIndex = pageIndex;
            // set the total of pages based in the number of rows left returned by the query or source 
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // append the elements returned by the query in the current list
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /**
         * With this static method we initialize a PaginatedList<T> object.
         * We execute this method every time we need a new page
         * -params-
         * source: the EF query
         * pageIndex: index in the pages. This index is not base 0!
         * pageSize: number of element in a page
         */
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
