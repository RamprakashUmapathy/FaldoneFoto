using Dapper;
using Kasanova.Common.ApplicationCore.Interfaces;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public interface IKeyItemValueRepository : IAsyncRepository<KeyItemValue, string>
    {
    }



    public class KeyItemValueRepository : BaseRepository, IKeyItemValueRepository
    {

        public IUnitOfWork DataContext { get; }

        public KeyItemValueRepository(IUnitOfWork context)
        {
            DataContext = context ?? throw new ArgumentNullException(nameof(context));
        }


        public Task<KeyItemValue> AddAsync(KeyItemValue entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(KeyItemValue entity)
        {
            throw new NotImplementedException();
        }

        public Task<KeyItemValue> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationInfo<KeyItemValue>> ListAllAsync(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationInfo<KeyItemValue>> ListAsync(dynamic parameters,int pageSize, int pageNumber)
        {

            using (var conn = DataContext.CreateConnection())
            {
                conn.Open();
                string spName = "faldone.KeyItemValuesList";
                var gridReader = await conn.QueryMultipleAsync(spName
                                                                , (object) parameters
                                                               , commandType: CommandType.StoredProcedure);

                var results = await gridReader.ReadAsync<KeyItemValue>(); 
                var recordCount = gridReader.Read<int>().First();
                return PaginationInfo<KeyItemValue>(pageSize, pageNumber, recordCount, results);
            }

        }

        public Task UpdateAsync(KeyItemValue entity)
        {
            throw new NotImplementedException();
        }
    }
}
