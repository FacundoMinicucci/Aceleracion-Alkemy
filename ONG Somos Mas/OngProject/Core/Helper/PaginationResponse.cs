using Newtonsoft.Json;

namespace OngProject.Core.Helper
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(GenericPagination<T> data)
        {
            Data = data;
            CurrentPage = data.CurrentPage;
            PageSize = data.PageSize;
            TotalPages = data.TotalPages;
            TotalRecords = data.TotalRecords;
            HasNextPage = data.HasNextPage;
            HasPreviousPage = data.HasPreviousPage;
            NextPageURL = data.NextPageURL;
            PreviousPageURL = data.PreviousPageURL;
        }

        public GenericPagination<T> Data { get; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        [JsonIgnore]
        public bool HasNextPage { get; set; }
        [JsonIgnore]
        public bool HasPreviousPage { get; set; }
        public string NextPageURL { get; set; }
        public string PreviousPageURL { get; set; }
    }
}
