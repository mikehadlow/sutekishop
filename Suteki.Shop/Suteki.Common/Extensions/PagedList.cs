//
// This code taken from Rob Conery's blog:
// http://blog.wekeroad.com/2007/12/10/aspnet-mvc-pagedlistt/
//

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Suteki.Common.Extensions
{
    public interface IPagedList
    {
        int TotalCount { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        bool IsPreviousPage { get; }
        bool IsNextPage { get; }
        int NumberOfPages { get; }
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageSize = pageSize;
            this.PageIndex = index;
            this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source, int index, int pageSize)
        {
            this.TotalCount = source.Count();
            this.PageSize = pageSize;
            this.PageIndex = index;
            this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return (PageIndex * PageSize) <= TotalCount;
            }
        }

        public int NumberOfPages
        {
            get
            {
                return (int)((TotalCount / PageSize) + 1);
            }
        }
    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static int PageNumber(this NameValueCollection formOrQuerystring)
        {
            if (formOrQuerystring == null) return 0;
            int pageNumber = 0;
            int.TryParse(formOrQuerystring["CurrentPage"], out pageNumber);
            return pageNumber;
        }
    }
}