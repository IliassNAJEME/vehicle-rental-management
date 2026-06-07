using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Services
{
    public class SqlServerDBFactory : InterfaceDbFactory
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                Initial Catalog=concessionnaireVoituresGrA;Integrated Security=True;
                                Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;
                                Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

        public DbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
