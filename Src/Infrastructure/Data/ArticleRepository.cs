using Dapper;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public interface IArticleRepository : IAsyncRepository<ChalcoArticle, string>
    {
    }

    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public IUnitOfWork DataContext { get; }

        public ArticleRepository(IUnitOfWork context)
        {
            DataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<ChalcoArticle> AddAsync(ChalcoArticle entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ChalcoArticle entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ChalcoArticle> GetByIdAsync(string id)
        {
            using (var conn = DataContext.CreateConnection())
            {
                conn.Open();
                string spName = "faldone.ArticlesRead";
                var gridReader = await conn.QueryMultipleAsync(spName
                                                                , new { Id = id}
                                                               , commandType: CommandType.StoredProcedure);
                var result = await gridReader.ReadSingleAsync<ChalcoArticle>();
                return result ;
            }
        }

        public async Task<PaginationInfo<ChalcoArticle>> ListAllAsync(int pageSize, int pageNumber)
        {
            using (var conn = DataContext.CreateConnection())
            {
                conn.Open();
                string spName = "faldone.ArticlesListAll";
                var gridReader = await conn.QueryMultipleAsync(spName
                                                                , new { PageSize = pageSize, PageNumber = pageNumber }
                                                               , commandType: CommandType.StoredProcedure);
                var results =  await gridReader.ReadAsync<ChalcoArticle>();
                var articleDict = results.ToDictionary(f => f.Id, f => f);

                var stockGroups = await gridReader.ReadAsync<Kasanova.ApplicationCore.Entities.StockGroup>();
                foreach (Kasanova.ApplicationCore.Entities.StockGroup stockGroup in stockGroups)
                {
                    if (articleDict.TryGetValue(stockGroup.ArticleId, out ChalcoArticle article))
                    {
                        article.StockGroups.Add(stockGroup);
                    }
                }
                var priceLists = await gridReader.ReadAsync<PriceList>();
                foreach (PriceList pl in priceLists)
                {
                    if (articleDict.TryGetValue(pl.ArticleId, out ChalcoArticle article))
                    {
                        article.PriceLists.Add(pl);
                    }
                }
                var recordCount = gridReader.Read<int>().First();
                return PaginationInfo<ChalcoArticle>(pageSize, pageNumber, articleDict.Count(), articleDict.Values);
            }
        }

        public async Task<PaginationInfo<ChalcoArticle>> ListAsync(dynamic parameters, int pageSize, int pageNumber)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
            //using (var conn = DataContext.CreateConnection())
            //{
            //    conn.Open();
            //    string spName = "faldone.ArticlesList";
            //    var gridReader = await conn.QueryMultipleAsync(spName
            //                                                    , (object)parameters
            //                                                   , commandType: CommandType.StoredProcedure);

            //    var results = await gridReader.ReadAsync<ChalcoArticle>();
            //    var recordCount = gridReader.Read<int>().First();
            //    return PaginationInfo<ChalcoArticle>(pageSize, pageNumber, recordCount, results);
            //}
        }

        public Task UpdateAsync(ChalcoArticle entity)
        {
            throw new NotImplementedException();
        }
    }
}
