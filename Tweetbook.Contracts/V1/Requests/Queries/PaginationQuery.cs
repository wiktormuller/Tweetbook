namespace Tweetbook.Contracts.Requests.Queries
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = SetPageSize(pageSize);
        }

        private int SetPageSize(int pageSize)
        {
            return pageSize > 100 ? 100 : pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}