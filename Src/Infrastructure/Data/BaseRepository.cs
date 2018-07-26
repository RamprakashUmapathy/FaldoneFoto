using System;
using System.Collections.Generic;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public abstract class BaseRepository
    {
        protected PaginationInfo<T> PaginationInfo<T>(int pageSize, int pageNumber, int recordCount, IEnumerable<T> values)
        {
            var totalPages = Math.Ceiling(((double)recordCount / pageSize));
            return new PaginationInfo<T>()
            {
                CurrentPage = pageNumber,
                Data = values,
                PageSize = pageSize > recordCount ? recordCount : pageSize,
                RecordCount = recordCount,
                TotalPages = (int)totalPages
            };
        }
    }
}
