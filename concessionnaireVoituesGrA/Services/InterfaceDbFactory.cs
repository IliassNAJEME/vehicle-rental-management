using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Services
{
    public interface InterfaceDbFactory
    {
        DbConnection CreateConnection();
    }
}
