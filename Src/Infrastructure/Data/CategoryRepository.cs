using Dapper;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public interface ICategoryRepository : IAsyncRepository<Category, string>
    {
    }

    public class CategoryRepository : BaseRepository, ICategoryRepository
    {

        public IUnitOfWork DataContext { get; }

        public CategoryRepository(IUnitOfWork context)
        {
            DataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Category> AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationInfo<Category>> ListAllAsync(int pageSize, int pageNumber)
        {
            using (var conn = DataContext.CreateConnection())
            {
                conn.Open();
                string spName = "faldone.ArticleCategoryList";
                var results = await conn.QueryMultipleAsync(spName
                                                                , new { PageSize = pageSize, PageNumber = pageNumber }
                                                               , commandType: CommandType.StoredProcedure);
                var res = await results.ReadAsync<CategoriesCollection.ResultSet>(); 
                var recordCount = results.Read<int>().First();
                IEnumerable<Category> coll = CategoriesCollection.BuildTree(res);
                return PaginationInfo<Category>(pageSize, pageNumber, recordCount, coll);
            }
        }

        public Task<PaginationInfo<Category>> ListAsync(dynamic parameters, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category entity)
        {
            throw new NotImplementedException();
        }

    }
}
