using Kasanova.FaldoneFoto.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Kasanova.FaldoneFoto.Infrastructure.Data
{
    public class DataContext : IUnitOfWork, IDisposable
    {

        private readonly string mConnectionString;
        private IDbConnection mConnection;

        public IDbConnection CreateConnection()
        {
            mConnection = new SqlConnection(mConnectionString);
            return mConnection;
        }

        bool m_Disposed = false;

        protected bool Disposed
        {
            get
            {
                lock (this)
                {
                    return m_Disposed;
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                //Check to see if Dispose() has already been called
                if (m_Disposed == false)
                {
                    Cleanup();
                    m_Disposed = true;
                    //Take yourself off the finalization queue 
                    //to prevent finalization from executing a second time.
                    GC.SuppressFinalize(this);
                }
            }
        }

        protected virtual void Cleanup()
        {
            if (mConnection != null) { mConnection.Close(); }
        }

        //Destructor will run only if Dispose() is not called.
        //Do not provide destructors in types derived from this class.
        ~DataContext()
        {
            Cleanup();
        }


        public DataContext(IConfiguration configuration)
        {
            mConnectionString = configuration.GetConnectionString("FaldoneFotoConnection");
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

    }

}
