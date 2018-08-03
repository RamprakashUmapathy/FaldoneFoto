using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection CreateConnection();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
