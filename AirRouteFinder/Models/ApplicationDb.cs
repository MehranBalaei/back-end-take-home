using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace AirRouteFinder.Models
{
    public static class ApplicationDb
    {
        static OleDbConnection connection;

        public static DbDataReader RunQuery(string queryString)
        {
            
            string appRootPath = AppDomain.CurrentDomain.BaseDirectory;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                connection = new OleDbConnection(connectionString);
                connection.Open();
            }
            OleDbCommand command = new OleDbCommand(queryString, connection);

            try
            {
                OleDbDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (connection != null)
                    connection.Close();

                return null;
            }
        }

        public static Dictionary<string,string> RunQueryAndReturnFirstRow(string queryString)
        {
            DbDataReader reader = null;
            try
            {
                reader = ApplicationDb.RunQuery(queryString);

                reader.Read();

                if (!reader.HasRows)
                    return null;
                      
                var result = new Dictionary<string, string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i), reader[i].ToString());
                }
                return result;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                //Closing the connection at the end.
                if (connection != null)
                    connection.Close();
            }
        }
    }
}