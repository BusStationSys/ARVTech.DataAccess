namespace ARVTech.DataAccess.Domain.Common
{
    using System.Collections.Generic;

    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get
            {
                return (TotalRecords + PageSize - 1) / PageSize;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}