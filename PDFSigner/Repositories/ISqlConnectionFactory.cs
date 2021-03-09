using System.Data;

namespace PDFSign.Repositories
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}