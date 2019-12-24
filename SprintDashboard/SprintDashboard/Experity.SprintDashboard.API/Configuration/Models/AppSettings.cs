using System.Data.SqlClient;
using System.Text;

namespace Experity.SprintDashboard.API.Configuration.Models
{
    public class AppSettings
    {
        public string ConnectionString { get; private set; }

        public string Secret { get; }
        public string ApplicationName { get; }
        public string ApplicationVersion { get; }
        public string HostName { get; }
        public string DataSource { get; }
        public string UserId { get; }
        public string Password { get; }
        public bool SwaggerEnable { get; }
        public bool IsDevEnv { get; } = true;
        public bool EnableLoggingEndpoint { get; } = false;

        public JwtSettingsModel ClaimsSettingsModel { get; }

        public AppSettings(string dataSource, string secret, string userId, string password,
            string hostName, bool swaggerEnable, string applicationVersion, JwtSettingsModel claims,
            bool enableLoggingEndpoint)
        {
            DataSource = dataSource;
            Secret = secret;
            UserId = userId;
            Password = password;
            ApplicationName = AppConstants.ApplicationName;
            HostName = hostName;
            SwaggerEnable = swaggerEnable;
            ApplicationVersion = applicationVersion;
            ClaimsSettingsModel = claims;
            EnableLoggingEndpoint = enableLoggingEndpoint;

            BuildConnectionString();
        }

        private void BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = DataSource,
                UserID = UserId,
                Password = Password,
                ApplicationName = $"{ApplicationName} {HostName} {ApplicationVersion}",
                InitialCatalog = "Messaging",
                PersistSecurityInfo = false,
                PacketSize = 4096,
                IntegratedSecurity = false
            };

            ConnectionString = builder.ConnectionString;
        }

        public override string ToString()
        {
            var settings = new StringBuilder();
            settings.AppendLine($"Secret length: {Secret?.Length ?? 0}");
            settings.AppendLine($"ApplicationName: {ApplicationName}");
            settings.AppendLine($"ApplicationVersion: {ApplicationVersion}");
            settings.AppendLine($"HostName: {HostName}");
            settings.AppendLine($"DataSource: {DataSource}");
            settings.AppendLine($"UserId: {UserId}");
            settings.AppendLine($"Password length: {Password?.Length ?? 0}");
            settings.AppendLine($"SwaggerEnable: {SwaggerEnable}");
            settings.AppendLine($"IsDevEnv: {IsDevEnv}");
            settings.AppendLine($"ConnectionString length: {ConnectionString?.Length ?? 0}");

            return settings.ToString();
        }
    }
}
