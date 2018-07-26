using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{

    public class ArticleRepositoryCache : ArticleRepository, IArticleRepository
    {

        private const string cacheKey = "Kasanova.FaldoneFoto.Articles.{0}.{1}";

        public ArticleRepositoryCache(DataContext context) : base(context) { }

        public new async Task<PaginationInfo<ChalcoArticle>> ListAllAsync(int pageSize, int pageNumber)
        {
            //MemoryCache cache = MemoryCache.Default;
            //string key = string.Format(cacheKey, pageSize, pageNumber);
            //PaginationInfo<ChalcoArticle> repositoryCache = cache[key] as PaginationInfo<ChalcoArticle>;
            //if (repositoryCache == null)
            //{
            //    //TODO Cache removal policy
            PaginationInfo<ChalcoArticle>  repositoryCache = await base.ListAllAsync(pageSize, pageNumber);
            //    cache.Add(key, repositoryCache, null);
            //}
            return repositoryCache;
        }

        public new async Task<PaginationInfo<ChalcoArticle>> ListAsync(dynamic parameters, int pageSize, int pageNumber)
        {
            //MemoryCache cache = MemoryCache.Default;
            //string key = string.Format(cacheKey, pageSize, pageNumber);
            //PaginationInfo<ChalcoArticle> repositoryCache = cache[key] as PaginationInfo<ChalcoArticle>;
            //if (repositoryCache == null)
            //{
            //    //TODO Cache removal policy
            PaginationInfo<ChalcoArticle>  repositoryCache = await base.ListAsync((object) parameters, pageSize, pageNumber);
            //    cache.Add(key, repositoryCache, null);
            //}
            return repositoryCache;
        }

        public async Task<IEnumerable<ChalcoArticle>> ListAsync(Func<ChalcoArticle, bool> predicate)
        {
            //Get all from cache
            var cache = await ListAllAsync(int.MaxValue, 1);

            var newResults = cache.Data.Where(predicate);
            return newResults;
        }

    }

}
