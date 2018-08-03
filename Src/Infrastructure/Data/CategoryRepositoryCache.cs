using Kasanova;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Web.Services
{
    public class CategoryRepositoryCache : CategoryRepository, ICategoryRepository
    {
        private IMemoryCache _cache;
        private const string cacheKey = "Kasanova.FaldoneFoto.TreeViewData.{0}.{1}";

        public CategoryRepositoryCache(IUnitOfWork context, IMemoryCache memoryCache) : base(context)
        {
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public new async Task<PaginationInfo<Category>> ListAllAsync(int pageSize, int pageNumber)
        {
            object key = string.Format(cacheKey, pageSize, pageNumber);
            var repositoryCache = await _cache.GetOrCreateAsync(key, async item =>
            {
                var categories = await base.ListAllAsync(pageSize, pageNumber);
                return await Task.FromResult<PaginationInfo<Category>>(categories);
            });
            return repositoryCache;
        }



    }
}