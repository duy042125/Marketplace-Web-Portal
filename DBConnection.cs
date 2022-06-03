using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Marketplace_Web_Portal.DB_Models;

namespace Marketplace_Web_Portal
{
    public class ConnectionStringProvider
    {
        public static string ApplicationExeDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
        }

        public static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = ApplicationExeDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(applicationExeDirectory)
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
    public class DBConnection
    {
        #region Private Components

        private static DBConnection DBInstance;
        private DBConnection() { }
        private static string getConnectionString(string name)
        {
            var appSettingsJson = ConnectionStringProvider.GetAppSettings();
            string connectionString = appSettingsJson[name];
            return connectionString;
        }

        #endregion

        public static DBConnection openConnection()
        {
            if (DBInstance == null) DBInstance = new DBConnection();
            return DBInstance;
        }

        #region Users Connections

        public static bool verifiedLogIn(string theEmail, string thePassword)
        {
            try 
            {
                using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(getConnectionString("UserDatabase")))
                {
                    string queries = "SP_Users_RetrieveByEmail";
                    var parameter = new DynamicParameters();
                    parameter.Add("@email", theEmail);

                    var data = connection.Query<Users>(queries, parameter, commandType: CommandType.StoredProcedure).ToList();
                    var currentUser = data.FirstOrDefault();
                    if (currentUser.password == thePassword) return true;
                    else return false;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
