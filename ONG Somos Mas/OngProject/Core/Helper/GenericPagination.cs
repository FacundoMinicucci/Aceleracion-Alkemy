using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OngProject.Core.Helper
{
    public class GenericPagination<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;
        public string NextPageURL { get; set; }
        public string PreviousPageURL { get; set; }
        public GenericPagination(List<T> items, int count, int pageNumber, int pageSize, string route, IUriService uriService)
        {
            TotalRecords = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (CurrentPage > TotalPages)
            {
                throw new ArgumentOutOfRangeException();
            }

            AddRange(items);

            NextPageURL = HasNextPage ? uriService.GetPage(route, NextPageNumber) : null;
            PreviousPageURL = HasPreviousPage ? uriService.GetPage(route, PreviousPageNumber) : null;
        }

        public static GenericPagination<T> Create(IEnumerable<T> source, int pageNumber, string route, IUriService uriService)
        {
            int pageSize = 10;
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new GenericPagination<T>(items, count, pageNumber, pageSize, route, uriService);
        }
    }
}
