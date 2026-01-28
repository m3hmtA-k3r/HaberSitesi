namespace WebUI.Models
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int StartPage => Math.Max(1, CurrentPage - 2);
        public int EndPage => Math.Min(TotalPages, CurrentPage + 2);
    }

    public static class PaginationHelper
    {
        public static List<T> GetPaginatedList<T>(List<T> source, int pageNumber, int pageSize)
        {
            return source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public static PaginationInfo GetPaginationInfo(int totalItems, int pageNumber, int pageSize)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new PaginationInfo
            {
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}
