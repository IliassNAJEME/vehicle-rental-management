using MySql.Data.MySqlClient;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Services
{
    public class MySqlDbFactory : InterfaceDbFactory
    {
        string urlConnecion = @"server=localhost;port=3306;
                                database=concessionnaireVoituresGrA;user=root;password=pwd";
        public DbConnection CreateConnection()
        {
            return new MySqlConnection(urlConnecion);
        }
    }
}
