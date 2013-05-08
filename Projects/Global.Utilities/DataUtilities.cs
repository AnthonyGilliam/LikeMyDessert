using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Global.Utilities
{
    public static class DataUtilities
    {
        #region Database Connection Handlers
        public static void OpenDatabaseConnection(SqlConnection dbConnection)
        {
            if (dbConnection.State != System.Data.ConnectionState.Open)
            {
                while (dbConnection.State != System.Data.ConnectionState.Open
                        && dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    throw new Exception("Database status is:  " + dbConnection.State.ToString());
                }

                dbConnection.Open();
            }
        }

        public static void CloseDatabaseConnection(SqlConnection dbConnection)
        {
            if (dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }
        #endregion Open Database Connection
    }
}
