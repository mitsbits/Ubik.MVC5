using System.Collections.Generic;

namespace Ubik.Infra.DataManagement
{
    public struct PagedResult<T> where T : class
    {
        public IEnumerable<T> Data;
        public int PageNumber;
        public int PageSize;
        public int TotalRecords;
        public int TotalPages;
    }
}