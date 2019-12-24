using System.Data.SqlClient;
using System.Diagnostics;
using Experity.SprintDashboard.Data.GenPro.Sql;
using SD.LLBLGen.Pro.DQE.SqlServer;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Experity.SprintDashboard.DataAccess.GenPro
{

    //Taken directly from Denials API.
    public static class AdapterFactory
    {
        public static IDataAccessAdapter CreateAdapter(string connectionString, string environment = "Common")
        {
            //have to do this stuff for Core
            RuntimeConfiguration.ConfigureDQE<SQLServerDQEConfiguration>(c => c
                .SetTraceLevel(TraceLevel.Verbose)
                .AddDbProviderFactory(typeof(SqlClientFactory))
                .SetDefaultCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2012));

            // we have to do this, otherwise the connection string will be overwritten correctly 
            // but the database name in the generated entities/stored procedure calls will not
            foreach (var overwrite in GetCatelogOverwriteHashTable(environment))
            {
                RuntimeConfiguration.ConfigureDQE<SQLServerDQEConfiguration>(c => c
                    .AddCatalogNameOverwrite(overwrite.Key, overwrite.Value));
            }

            return GetStandardAdapter(connectionString, environment);
        }

        private static IDataAccessAdapter GetStandardAdapter(string connectionString, string environment)
        {
            var overWrites = GetCatelogOverwriteHashTable(environment);
            var conString = BuildConnectionStringForEnvironment(connectionString, environment);

            return new DataAccessAdapter(conString, false, overWrites, null);
        }

        private static CatalogNameOverwriteHashtable GetCatelogOverwriteHashTable(string environment)
        {
            return new CatalogNameOverwriteHashtable
            {
                {"__Master_Practice", environment},
                {"__Master_Common", "Common"},
            };
        }

        /// <summary>Build stack-aware connection string for given evironment</summary>
        /// <param name="environment">DB environment</param>
        /// <param name="connectionString">Current conn string that will values replaced if not accessing Common DB</param>
        /// <returns>Conn string</returns>
        private static string BuildConnectionStringForEnvironment(string connectionString, string environment)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = environment;
            return builder.ConnectionString;
        }
    }
}