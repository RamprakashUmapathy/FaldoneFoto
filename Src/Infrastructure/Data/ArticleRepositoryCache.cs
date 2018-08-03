using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{

    public class ArticleRepositoryCache : ArticleRepository, IArticleRepository
    {
        private IMemoryCache _cache;
        private const string cacheKey = "Kasanova.FaldoneFoto.Articles.{0}.{1}";

        public ArticleRepositoryCache(IUnitOfWork context, IMemoryCache memoryCache) : base(context)
        {
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public new async Task<PaginationInfo<ChalcoArticle>> ListAllAsync(int pageSize, int pageNumber)
        {
            object key = string.Format(cacheKey, pageSize, pageNumber);
            var repositoryCache = await _cache.GetOrCreateAsync(key, async item =>
            {
                var articles = await base.ListAllAsync(pageSize, pageNumber);
                return await Task.FromResult<PaginationInfo<ChalcoArticle>>(articles);
            });
            return repositoryCache;
        }

        public new async Task<PaginationInfo<ChalcoArticle>> ListAsync(dynamic parameters, int pageSize, int pageNumber)
        {
            object key = string.Format(cacheKey, pageSize, pageNumber);
            var repositoryCache = await _cache.GetOrCreateAsync<PaginationInfo<ChalcoArticle>>(key, async item =>
           {
               var articles = await base.ListAsync((object)parameters, pageSize, pageNumber);
               return await  Task.FromResult<PaginationInfo<ChalcoArticle>>(articles);
           });
            return repositoryCache;
        }

        public async Task<IEnumerable<ChalcoArticle>> ListAsync(Func<ChalcoArticle, bool> predicate)
        {
            //Get all from cache and use predicate in memory data
            var cache = await ListAllAsync(int.MaxValue, 1);
            var newResults = cache.Data.Where(predicate);
            return newResults;
        }

    }

}
