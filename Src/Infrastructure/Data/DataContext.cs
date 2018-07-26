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

        //Do not make Dispose() virtual - you should prevent subclasses from overriding
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


        public DataContext(ConnectDB connectionName, IConfiguration configuration)
        {
            string name = null;
            switch (connectionName)
            {
                case ConnectDB.SqlServerNegozi:
                    {
                        throw new NotSupportedException();
                    }
                case ConnectDB.LocalDB:
                    {
                        name = "FaldoneFotoConnection";
                        break;
                    }
                case ConnectDB.ServerDWH:
                    throw new NotSupportedException();
                default:
                    throw new NotSupportedException();
            }
            mConnectionString = configuration.GetConnectionString(name);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

    }

}
