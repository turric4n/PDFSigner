using PDFSign.Repositories;
using System;
using System.Data;
using System.Data.SQLite;

namespace PDFSign.Database.Factories
{
    public class DbConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public DbConnectionFactory(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                this._connection = new SQLiteConnection(_connectionString);
                
                this._connection.Open();
            }

            return this._connection;
        }

        public void Dispose()
        {
            if (this._connection != null && this._connection.State == ConnectionState.Open)
            {
                this._connection.Dispose();
            }
        }
    }
}
