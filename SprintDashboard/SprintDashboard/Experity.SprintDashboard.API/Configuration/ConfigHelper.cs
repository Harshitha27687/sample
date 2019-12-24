using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Experity.SprintDashboard.API.Configuration
{
    public class ConfigHelper
    {
        public static IConfigurationRoot ConfigureFromAppSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNET_ENV");

            if (string.IsNullOrEmpty(environmentName))
            {
                environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }

            if (string.IsNullOrEmpty(environmentName))
            {
                environmentName = "Production";
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true)
                .AddJsonFile("launchSettings.json", true, true)
                .AddJsonFile($"launchSettings.{environmentName}.json", true, true)
                .AddJsonFile($"launchSettings.{Environment.MachineName}.json", true, true)
                .Build();

            return config;
        }
    }
}
