using Microsoft.Extensions.Configuration;

namespace Experity.SprintDashboard.API.Configuration
{
    public abstract class ConfigurationLoaderBase
    {
        private readonly IConfiguration _config;
        public IConfiguration Config => _config;

        protected ConfigurationLoaderBase(IConfiguration config)
        {
            _config = config;
        }

        protected string GetSingleEnvVariable(string whichVar)
        {
            return _config.GetSection(whichVar).Value ?? string.Empty;
        }
    }
}
