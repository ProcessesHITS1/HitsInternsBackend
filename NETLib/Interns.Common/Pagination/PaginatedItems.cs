namespace Interns.Common.Pagination
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginatedItems<T>
    {
        public required PaginationInfo PaginationInfo { get; set; }
        public required IEnumerable<T> Items { get; set; }
    }
}
