using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Kasanova.FaldoneFoto.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.Api
{
    [Produces("application/json")]
    public class CategoriesController : Controller
    {
        private IAppLogger<CategoriesController> _logger;
        private ICategoryRepository _categoryRepository;
        private IKeyItemValueRepository _itemValueRepository;

        public CategoriesController(IAppLogger<CategoriesController> logger, ICategoryRepository categoryRepository, IKeyItemValueRepository itemValueRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _itemValueRepository = itemValueRepository ?? throw new ArgumentNullException(nameof(itemValueRepository));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var categories = new List<Category>();
            int pageSize = int.MaxValue; // no paging needed
            var pageInfo = await _categoryRepository.ListAllAsync(pageSize, 1);
            categories.AddRange(pageInfo.Data);
            for (int i = 2; i <= pageInfo.TotalPages; i++)
            {
                await _categoryRepository.ListAllAsync(pageSize, i);
                categories.AddRange(pageInfo.Data);
            }
            watch.Stop();
            _logger.LogInformation("{0} method executed in {1} seconds", MethodBase.GetCurrentMethod().Name, watch.Elapsed.TotalSeconds);
            return categories;
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


        /*
                            public async Task<IEnumerable<PriceList>> GetPriceListsAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
                            {
                                var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
                                if (!string.IsNullOrEmpty(level2Id))
                                {
                                    return results.GetPriceLists().Where(l => l.Id == level2Id);
                                }
                                return results.GetPriceLists();
                            }

                            public async Task<IEnumerable<StockGroup>> GetStockGroupsAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
                            {
                                var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
                                if (!string.IsNullOrEmpty(level2Id))
                                {
                                    return results.GetStockGroups().Where(l => l.Id == level2Id);
                                }
                                return results.GetStockGroups();
                            }

                            public async Task<IEnumerable<SupplyStatus>> GetSupplyStatusAsync(string categoryId, string familyId, string seriesId, string level1Id, string level2Id)
                            {
                                var results = await GetLevel2Async(categoryId, familyId, seriesId, level1Id, level2Id);
                                if (!string.IsNullOrEmpty(level2Id))
                                {
                                    return results.GetSupplyStatuses().Where(l => l.Id == level2Id);
                                }
                                return results.GetSupplyStatuses();
                            }
                            */
    }
}