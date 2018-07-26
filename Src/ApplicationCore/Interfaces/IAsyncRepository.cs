
using System.Threading.Tasks;

namespace Kasanova.Common.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T, Key>
    {
        Task<T> GetByIdAsync(Key id);
        Task<PaginationInfo<T>> ListAllAsync(int pageSize, int pageNumber);
        Task<PaginationInfo<T>> ListAsync(dynamic parameters, int pageSize, int pageNumber);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
