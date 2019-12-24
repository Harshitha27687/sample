using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Experity.SprintDashboard.API.Configuration;
using Experity.SprintDashboard.API.Configuration.ConfigurationKeys;

namespace Experity.SprintDashboard.API.Authentication
{
    public class AuthApiConfigLoader : ConfigurationLoaderBase
    {
        public AuthApiConfigLoader(IConfiguration config) : base(config)
        {
        }

        public AuthApiConfig LoadAuthConfig()
        {
            return new AuthApiConfig
            {
                ApiName = AuthApiKeys.ApiName,
                BaseAddress = GetSingleEnvVariable(AuthApiKeys.BaseAddress),
                HeaderDefinitions = new Dictionary<string, string>()
                {
                    {"Accept", "application/json"}
                },
                BaseRef = GetSingleEnvVariable(AuthApiKeys.ApiName)
            };
        }
    }
}
