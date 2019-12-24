using Microsoft.Extensions.Configuration;
using Experity.SprintDashboard.API.Configuration.Models;

namespace Experity.SprintDashboard.API.Configuration
{
    public class JwtConfigLoader : ConfigurationLoaderBase
    {

        private const string SigningPublicKeyFileName = "SigningPublicKeyFileName";
        private const string SigningPublicKeyFilePath = "SigningPublicKeyFilePath";
        private const string JwtIssuerKey = "JwtIssuer";
        private const string JwtAudienceKey = "JwtAudience";
        private const string JwtExpirationMinutesKey = "JwtExpirationMinutes";
        private const string JwtAppNameClaimKey = "ApplicationName";
        private const string JwtPermissionsClaimKey = "ApplicationPermissions";

        public JwtConfigLoader(IConfiguration config) : base(config)
        {
        }

        public JwtSettingsModel LoadJwtSettings()
        {
            if (!int.TryParse(GetSingleEnvVariable(JwtExpirationMinutesKey), out int expMin))
            {
                expMin = 60;
            }

            return new JwtSettingsModel()
            {
                PublicKeyFileName = GetSingleEnvVariable(SigningPublicKeyFileName),
                PublicKeyFilePath = GetSingleEnvVariable(SigningPublicKeyFilePath),
                Issuer = GetSingleEnvVariable(JwtIssuerKey),
                Audience = GetSingleEnvVariable(JwtAudienceKey),
                JwtAppNameClaim = GetSingleEnvVariable(JwtAppNameClaimKey),
                JwtPermissionsClaim = GetSingleEnvVariable(JwtPermissionsClaimKey),
                ExpirationInMinutes = expMin
            };
        }
    }
}
