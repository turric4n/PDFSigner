using Dapper;
using PDFSign.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PDFSign.Repositories
{
    public class CertificateDataRepository : ICertificateDataRepository
    {
        private readonly IDbConnection _dbConnection;

        public CertificateDataRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _dbConnection = sqlConnectionFactory.GetOpenConnection();
        }

        public void Add(CertificateData Entity)
        {
            var parameters = new { Password = Entity.Password, Path = Entity.Path, BusinessName = Entity.BusinessName };
            _dbConnection.Execute($"INSERT INTO CertificateData (Password, Path, BusinessName) VALUES (@Password," +
                $"@Path," +
                $"@BusinessName)", parameters);
        }

        public void Delete(CertificateData Entity)
        {
            _dbConnection.Execute($"DELETE FROM CertificateData WHERE Id = { Entity.Id }");
        }

        public IEnumerable<CertificateData> GetAll()
        {
            
            var query = "SELECT * FROM CertificateData";
            return _dbConnection.Query<CertificateData>(query);                                
        }

        public CertificateData GetById(int Id)
        {
            var query = $"SELECT * FROM CertificateData WHERE Id={Id}";
            return _dbConnection.Query<CertificateData>(query).FirstOrDefault();
        }

        public CertificateData GetByName(string Name)
        {
            var parameters = new { BusinessName = Name };
            var query = $"SELECT * FROM CertificateData WHERE BusinessName = @BusinessName";
            return _dbConnection.Query<CertificateData>(query, parameters).FirstOrDefault();
        }

        public void Update(CertificateData Entity)
        {
            var parameters = new { Password = Entity.Password, Path = Entity.Path, BusinessName = Entity.BusinessName };
            _dbConnection.Execute($"UPDATE CertificateData SET Password = @Password," +
                $" Path = @Path," +
                $" BusinessName = @BusinessName" +
                $" WHERE Id = { Entity.Id }", parameters);
        }
    }
}
