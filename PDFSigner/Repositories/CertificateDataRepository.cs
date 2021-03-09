using PDFSign.Models;
using System.Collections.Generic;
using Dapper;
using Dapper.Contrib;
using System.Data;
using System.Linq;

namespace PDFSign.Repositories
{
    public class CertificateDataRepository : ICertificateDataRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IDbConnection _dbconnection;

        public CertificateDataRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _dbconnection = _sqlConnectionFactory.GetOpenConnection();
        }

        public void Add(CertificateData Entity)
        {
            var parameters = new { Password = Entity.Password, Path = Entity.Path, BusinessName = Entity.Businessname };
            _dbconnection.Execute($"INSERT INTO CertificateData (Password, Path, BusinessName) VALUES (@Password," +
                $"@Path," +
                $"@BusinessName)", parameters);
        }

        public void Delete(CertificateData Entity)
        {
            _dbconnection.Execute($"DELETE FROM CertificateData WHERE Id = { Entity.Id }");
        }

        public IEnumerable<CertificateData> GetAll()
        {
            
            var query = "SELECT * FROM CertificateData";
            return _dbconnection.Query<CertificateData>(query);                                
        }

        public CertificateData GetById(int Id)
        {
            var query = $"SELECT * FROM CertificateData WHERE Id={Id}";
            return _dbconnection.Query<CertificateData>(query).FirstOrDefault();
        }

        public CertificateData GetByName(string Name)
        {
            throw new System.NotImplementedException();
        }

        public void Update(CertificateData Entity)
        {
            var parameters = new { Password = Entity.Password, Path = Entity.Path, BusinessName = Entity.Businessname };
            _dbconnection.Execute($"UPDATE CertificateData SET Password = @Password," +
                $" Path = @Path," +
                $" BusinessName = @BusinessName" +
                $" WHERE Id = { Entity.Id }", parameters);
        }
    }
}
