using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataAccess
{
    public class DBConnection
    {
        public static SqlConnection GetConnection() 
        {
            //school
            var connString = @"Data Source=localhost;Initial Catalog=eventDB;Integrated Security=True";

            // home
            //var connString = @"Data Source=localhost\SQLCLOPEZ;Initial Catalog=eventDB;Integrated Security=True";

            // work
            //var connString = @"Data Source=server003447;Initial Catalog=eventDB;Integrated Security=True";
            return new SqlConnection(connString);
        }
    }
}
