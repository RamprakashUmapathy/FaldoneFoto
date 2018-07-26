using Kasanova;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Web.Services
{
    public class CategoryRepositoryCache : CategoryRepository, ICategoryRepository
    {
        private IMemoryCache _cache;
        private const string cacheKey = "Kasanova.FaldoneFoto.TreeViewData.{0}.{1}";

        public CategoryRepositoryCache(DataContext context, IMemoryCache memoryCache) : base(context) { }

        public new async Task<PaginationInfo<Category>> ListAllAsync(int pageSize, int pageNumber)
        {
            object key = string.Format(cacheKey,pageSize, pageNumber);
            PaginationInfo<Category> repositoryCache = _cache.Get<PaginationInfo<Category>>(key);
            if (repositoryCache == null)
            {
                repositoryCache = await base.ListAllAsync(pageSize, pageNumber);
                var cacheEntry = _cache.CreateEntry(key);
                cacheEntry.Value = repositoryCache;
            }
            return repositoryCache;
        }



    }
}