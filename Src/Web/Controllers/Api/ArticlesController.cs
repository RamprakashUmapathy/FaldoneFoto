using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Web.Extensions;

namespace Web.Controllers.Api
{
    [Produces("application/json")]
    public class ArticlesController : Controller
    {
        private IAppLogger<ArticlesController> _logger;
        private IArticleRepository _articleRepository;
        private IKeyItemValueRepository _itemValueRepository;

        public ArticlesController(IAppLogger<ArticlesController> logger, IArticleRepository articleRepository, IKeyItemValueRepository itemValueRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _itemValueRepository = itemValueRepository ?? throw new ArgumentNullException(nameof(itemValueRepository));
        }

        /// <summary>
        /// GET : api/articles/{shopsignid}/{categoryid}/{familyid?}/{seriesid?}/{level1id?}/{level2id?}/{styleid?}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <param name="level2Id"></param>
        /// <param name="styleid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Article>> GetArticlesAsync([FromRoute]string shopsignId, [FromRoute]string categoryId, [FromRoute]string familyId, [FromRoute]string seriesId, [FromRoute]string level1Id, [FromRoute]string level2Id, [FromRoute]string styleid)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var stockGroups = GetStockGroupsFromShopSign(shopsignId);

            var cache = (ArticleRepositoryCache)_articleRepository;
            var articles = await cache.ListAsync(f =>
                (string.IsNullOrEmpty(categoryId) || f.Category == categoryId) &&
                (string.IsNullOrEmpty(familyId) || f.Family == familyId) &&
                (string.IsNullOrEmpty(seriesId) || f.Series == seriesId) &&
                (string.IsNullOrEmpty(level1Id) || f.Level1 == level1Id) &&
                (string.IsNullOrEmpty(level2Id) || f.Level2 == level2Id) &&
                f.StockGroups.Any(s => stockGroups.Contains(s.StockGroupId)));
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", "Get with parameters", watch.Elapsed.TotalSeconds);
            return articles;
        }

        /// <summary>
        /// GET : api/shopsigns
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ShopSign>> GetShopSignsAsync()
        {

            var categories = await GetArticleCategoriesAsync();
            List<ShopSign> list = new List<ShopSign>();
            var signs = new string[] { "KASANOVA", "KASANOVA+", "COIMPORT", "L'Outlet", "Casa sulla'albero", "Le Kikke", "E-Commerce" };

            foreach (string sign in signs)
            {
                var stockGroups = GetStockGroupsFromShopSign(sign);
                ShopSign shop = new ShopSign() { Id = sign };
                var r = categories.Where(c => string.Join(',', stockGroups) == c.StockGroupId);
                shop.Categories.AddRange(CategoriesCollection.BuildTree(r));
                list.Add(shop);
            }
            return list;
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories
        /// </summary>
        /// <param name="shopsignId">shop sign id to extract</param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetCategoriesAsync([FromRoute] string shopsignId)
        {
            var categories = await GetArticleCategoriesAsync();
            var stockGroups = GetStockGroupsFromShopSign(shopsignId);
            ShopSign shop = new ShopSign() { Id = shopsignId };
            var r = categories.Where(c => string.Join(',', stockGroups) == c.StockGroupId);
            return CategoriesCollection.BuildTree(r);
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryId}
        /// </summary>
        /// <param name="shopsignId">shop sign id to extract</param>
        /// <param name="categoryId">category id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetCategoriesByIdAsync([FromRoute] string shopsignId, [FromRoute]  string categoryId)
        {
            var categories = await GetCategoriesAsync(shopsignId);
            if (!string.IsNullOrEmpty(categoryId))
            {
                return categories.Where(c => c.Id == categoryId);
            }
            return categories;
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Family>> GetFamiliesAsync([FromRoute] string shopsignId, [FromRoute] string categoryId)
        {
            var results = await GetCategoriesByIdAsync(shopsignId, categoryId);
            return results.GetFamilies();
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Family>> GetFamiliesByIdAsync([FromRoute] string shopsignId, [FromRoute] string categoryId, [FromRoute] string familyId)
        {
            var results = await GetFamiliesAsync(shopsignId, categoryId);
            if (!string.IsNullOrEmpty(familyId))
            {
                return results.Where(c => c.Id == familyId);
            }
            return results;
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Series>> GetSeriesAsync([FromRoute] string shopsignId, [FromRoute] string categoryId, [FromRoute] string familyId)
        {
            var results = await GetFamiliesByIdAsync(shopsignId, categoryId, familyId);
            return results.GetSeries();
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Series>> GetSeriesByIdAsync([FromRoute] string shopsignId, [FromRoute] string categoryId, [FromRoute] string familyId, [FromRoute] string seriesId)
        {
            var results = await GetFamiliesByIdAsync(shopsignId, categoryId, familyId);
            if (!string.IsNullOrEmpty(seriesId))
            {
                return results.GetSeries().Where(f => f.Id == seriesId);
            }
            return results.GetSeries();
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Level1>> GetLevel1Async([FromRoute] string shopsignId, [FromRoute] string categoryId, [FromRoute] string familyId, [FromRoute] string seriesId)
        {
            var results = await GetSeriesByIdAsync(shopsignId, categoryId, familyId, seriesId);
            return results.GetLevel1s();
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Level1>> GetLevel1ByIdAsync(string shopsignId, string categoryId, string familyId, string seriesId, string level1Id)
        {
            var results = await GetSeriesByIdAsync(shopsignId, categoryId, familyId, seriesId);
            if (!string.IsNullOrEmpty(level1Id))
            {
                return results.GetLevel1s().Where(f => f.Id == level1Id);
            }
            return results.GetLevel1s();
        }

        /// <summary>
        /// GET : api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Level2>> GetLevel2Async(string shopsignId, string categoryId, string familyId, string seriesId, string level1Id)
        {
            var results = await GetLevel1ByIdAsync(shopsignId, categoryId, familyId, seriesId, level1Id);
            return results.GetLevel2s();
        }

        /// <summary>
        /// GET api/shopsigns/{shopsignid}/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <param name="level2Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Level2>> GetLevel2ByIdAsync(string shopsignId, string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        {
            var results = await GetLevel1ByIdAsync(shopsignId, categoryId, familyId, seriesId, level1Id);
            if (!string.IsNullOrEmpty(level2Id))
            {
                return results.GetLevel2s().Where(l => l.Id == level2Id);
            }
            return results.GetLevel2s();
        }

        /// <summary>
        /// GET : api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <param name="level2Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Style>> GetStylesAsync(string shopsignId, string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        {
            var results = await GetLevel2ByIdAsync(shopsignId, categoryId, familyId, seriesId, level1Id, level2Id);
            return results.GetStyles();
        }

        /// <summary>
        /// GET : api/categories/{categoryid}/families/{familyid}/series/{seriesid}/level1/{level1id}/level2/{level2id}/styles/{styleid}
        /// </summary>
        /// <param name="shopsignId"></param>
        /// <param name="categoryId"></param>
        /// <param name="familyId"></param>
        /// <param name="seriesId"></param>
        /// <param name="level1Id"></param>
        /// <param name="level2Id"></param>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Style>> GetStylesByIdAsync(string shopsignId, string categoryId, string familyId, string seriesId, string level1Id, string level2Id, string styleId)
        {
            var results = await GetLevel2ByIdAsync(shopsignId, categoryId, familyId, seriesId, level1Id, level2Id);
            if (!string.IsNullOrEmpty(styleId))
            {
                return results.GetStyles().Where(l => l.Id == styleId);
            }
            return results.GetStyles();
        }

        //public async Task<IEnumerable<PriceList>> GetPriceListsAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        //{
        //    var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
        //    if (!string.IsNullOrEmpty(level2Id))
        //    {
        //        return results.GetPriceLists().Where(l => l.Id == level2Id);
        //    }
        //    return results.GetPriceLists();
        //}

        //public async Task<IEnumerable<StockGroup>> GetStockGroupsAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        //{
        //    var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
        //    if (!string.IsNullOrEmpty(level2Id))
        //    {
        //        return results.GetStockGroups().Where(l => l.Id == level2Id);
        //    }
        //    return results.GetStockGroups();
        //}

        //public async Task<IEnumerable<SupplyStatus>> GetSupplyStatusAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        //{
        //    var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
        //    if (!string.IsNullOrEmpty(level2Id))
        //    {
        //        return results.GetSupplyStatuses().Where(l => l.Id == level2Id);
        //    }
        //    return results.GetSupplyStatuses();
        //}

        #region private methods

        private IEnumerable<string> GetStockGroupsFromShopSign(string shopsignId)
        {
            List<string> list = new List<string>();
            switch (shopsignId)
            {
                case "KASANOVA":
                    list.AddRange(new string[] { "KAS02", "KAS04", "KAS05" });
                    break;
                case "KASANOVA+":
                    list.AddRange(new string[] { "KAS03", "KAS05" });
                    break;
                case "COIMPORT":
                    list.AddRange(new string[] { "COI01", "COI02", "COI03" });
                    break;
                case "L'Outlet":
                    list.AddRange(new string[] { "OUT01", });
                    break;
                case "Casa sulla'albero":
                    list.AddRange(new string[] { "CSA01" });
                    break;
                case "Le Kikke":
                    list.AddRange(new string[] { "KAS03", "KAS04", "KAS05" });
                    break;
                case "E-Commerce":
                    list.AddRange(new string[] { "WEB01" });
                    break;
                default:
                    throw new NotSupportedException();
            }
            return list;
        }

        private async Task<IEnumerable<CategoriesCollection.ResultSet>> GetArticleCategoriesAsync()
        {
            var articles = new List<Article>();
            int pageSize = int.MaxValue; // no paging needed for cache
            var pageInfo = await _articleRepository.ListAllAsync(pageSize, 1);
            articles.AddRange(pageInfo.Data);
            for (int i = 2; i <= pageInfo.TotalPages; i++)
            {
                await _articleRepository.ListAllAsync(pageSize, i);
                articles.AddRange(pageInfo.Data);
            }
            var categories = articles.DistinctBy(f => new
            {
                f.Category,
                f.Family,
                f.Series,
                f.Level1,
                f.Level2,
                f.Style,
                f.PriceListNames
            })
            .Select(f => new CategoriesCollection.ResultSet
            {
                CategoryId = f.Category,
                FamilyId = f.Family,
                SeriesId = f.Series,
                Level1Id = f.Level1,
                Level2Id = f.Level2,
                PriceListId = f.PriceListNames,
                StockGroupId = f.StockGroupNames,
                StyleId = f.Style,
                SupplyingStatusId = "",
                TagId = f.TagName
            });
            return categories;
        }

        #endregion

    }
}