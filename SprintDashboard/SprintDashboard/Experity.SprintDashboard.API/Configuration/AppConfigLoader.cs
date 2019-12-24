using Experity.SprintDashboard.API.Configuration.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace Experity.SprintDashboard.API.Configuration
{
    public class AppConfigLoader : ConfigurationLoaderBase
    {
        private const string DataSourceKey = "DataSource";
        private const string DbUserIdKey = "DbUserId";
        private const string DbPasswordKey = "DbPassword";
        private const string HostNameKey = "HostName";
        private const string ApplicationVersionKey = "ApplicationVersion";
        private const string SwaggerEnableKey = "SwaggerEnable";
        private const string SecretKey = "Secret";
        private const string EnableLoggingEndpointKey = "LoggingEndpointEnabled";

        public AppConfigLoader(IConfiguration config) : base(config)
        {
        }

        public AppSettings LoadSettings()
        {

            var dataSource = GetSingleEnvVariable(DataSourceKey);
            var secret = GetSingleEnvVariable(SecretKey);
            var userId = GetSingleEnvVariable(DbUserIdKey);
            var password = GetSingleEnvVariable(DbPasswordKey);
            var hostName = GetSingleEnvVariable(HostNameKey);
            var swaggerEnable = GetSingleEnvVariable(SwaggerEnableKey)
                                             .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            var applicationVersion = GetSingleEnvVariable(ApplicationVersionKey);

            var claimsSettingsModel = new JwtConfigLoader(base.Config).LoadJwtSettings();
            var enableLoggingEndpoint = GetSingleEnvVariable(EnableLoggingEndpointKey)
                .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            return new AppSettings(dataSource, secret, userId, password, hostName, swaggerEnable,
                                              applicationVersion, claimsSettingsModel, enableLoggingEndpoint);
        }
    }
}
