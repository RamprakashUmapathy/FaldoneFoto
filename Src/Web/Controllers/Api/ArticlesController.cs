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

        public async Task<IEnumerable<Article>> GetArticlesAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var cache = (ArticleRepositoryCache)_articleRepository;
            var articles = await cache.ListAsync(f =>
                (string.IsNullOrEmpty(categoryId) || f.Category == categoryId) &&
                (string.IsNullOrEmpty(familyId) || f.Family == familyId) &&
                (string.IsNullOrEmpty(seriesId) || f.Series == seriesId) &&
                (string.IsNullOrEmpty(level1Id) || f.Level1 == level1Id) &&
                (string.IsNullOrEmpty(level2Id) || f.Level2 == level2Id)
            );
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", "Get with parameters", watch.Elapsed.TotalSeconds);
            return articles;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var articles = new List<Article>();
            int pageSize = int.MaxValue; // no paging needed for cache
            var pageInfo = await _articleRepository.ListAllAsync(pageSize, 1);
            articles.AddRange(pageInfo.Data);
            for (int i = 2; i <= pageInfo.TotalPages; i++)
            {
                await _articleRepository.ListAllAsync(pageSize, i);
                articles.AddRange(pageInfo.Data);
            }
            var stockGroups = GetStockGroupsFromShopSign();
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
            .Where(f => f.StockGroups.Any(i => stockGroups.Contains(i.StockGroupId)))
            .Select(f => new CategoriesCollection.ResultSet
            {
                CategoryId = f.Category,
                FamilyId = f.Family,
                SeriesId = f.Series,
                Level1Id = f.Level1,
                Level2Id = f.Level2,
                PriceListId = f.PriceListNames,
                StockGroupId = "",
                StyleId = f.Style,
                SupplyingStatusId = "",
                TagId = f.TagName
            });
            var results = CategoriesCollection.BuildTree(categories);
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", MethodBase.GetCurrentMethod().Name, watch.Elapsed.TotalSeconds);
            return results;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByIdAsync(string categoryId)
        {
            var categories = await GetCategoriesAsync();
            if (!string.IsNullOrEmpty(categoryId))
            {
                return categories.Where(c => c.Id == categoryId);
            }
            return categories;
        }

        public async Task<IEnumerable<Family>> GetFamiliesAsync(string categoryId)
        {
            var results = await GetCategoriesByIdAsync(categoryId);
            return results.GetFamilies();
        }

        public async Task<IEnumerable<Family>> GetFamiliesByIdAsync(string categoryId, string familyId)
        {
            var results = await GetFamiliesAsync(categoryId);
            if (!string.IsNullOrEmpty(familyId))
            {
                return results.Where(c => c.Id == familyId);
            }
            return results;
        }

        public async Task<IEnumerable<Series>> GetSeriesAsync(string categoryId, string familyId)
        {
            var results = await GetFamiliesByIdAsync(categoryId, familyId);
            return results.GetSeries();
        }

        public async Task<IEnumerable<Series>> GetSeriesByIdAsync(string categoryId, string familyId, string seriesId)
        {
            var results = await GetFamiliesByIdAsync(categoryId, familyId);
            if (!string.IsNullOrEmpty(seriesId))
            {
                return results.GetSeries().Where(f => f.Id == seriesId);
            }
            return results.GetSeries();
        }

        public async Task<IEnumerable<Level1>> GetLevel1Async(string categoryId, string familyId, string seriesId)
        {
            var results = await GetSeriesByIdAsync(categoryId, familyId, seriesId);
            return results.GetLevel1s();
        }

        public async Task<IEnumerable<Level1>> GetLevel1ByIdAsync(string categoryId, string familyId, string seriesId, string level1Id)
        {
            var results = await GetSeriesByIdAsync(categoryId, familyId, seriesId);
            if (!string.IsNullOrEmpty(level1Id))
            {
                return results.GetLevel1s().Where(l => l.Id == level1Id);
            }
            return results.GetLevel1s();
        }

        public async Task<IEnumerable<Level2>> GetLevel2Async(string categoryId, string familyId, string seriesId, string level1Id)
        {
            var results = await GetLevel1ByIdAsync(categoryId, familyId, seriesId, level1Id);
            return results.GetLevel2s();
        }

        public async Task<IEnumerable<Level2>> GetLevel2ByIdAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        {
            var results = await GetLevel1ByIdAsync(categoryId, familyId, seriesId, level1Id);
            if (!string.IsNullOrEmpty(level2Id))
            {
                return results.GetLevel2s().Where(l => l.Id == level2Id);
            }
            return results.GetLevel2s();
        }

        public async Task<IEnumerable<Style>> GetStylesAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
        {
            var results = await GetLevel2ByIdAsync(categoryId, familyId, seriesId, level1Id, level2Id);
            return results.GetStyles();
        }

        public async Task<IEnumerable<Style>> GetStylesByIdAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id, string styleId)
        {
            var results = await GetLevel2ByIdAsync(categoryId, familyId, seriesId, level1Id, level2Id);
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


        private IEnumerable<string> GetStockGroupsFromShopSign()
        {
            List<string> list = new List<string>();
            switch (Request.Headers["SHOPSIGN"])
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
                    list.AddRange(new string[] { "OUT01",});
                    break;
                case "Casa sulla'albero":
                    list.AddRange(new string[] { "CSA01"});
                    break;
                case "Le Kikke":
                    list.AddRange(new string[] { "KAS03", "KAS04", "KAS05" });
                    break;
                case "E-Commerce":
                    list.AddRange(new string[] { "WEB01"});
                    break;
                default:
                    throw new NotSupportedException();
            }
            return list;
        }
    }
}