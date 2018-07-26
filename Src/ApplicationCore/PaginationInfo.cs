using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasanova
{
    public class PaginationInfo<T>
    {
        public int RecordCount { get;  set; }

        public int PageSize { get;  set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<T> Data { get; set; }

    }
}
