using Microsoft.Extensions.Configuration;
using Experity.SprintDashboard.API.Configuration;
using Experity.SprintDashboard.API.Configuration.ConfigurationKeys;

namespace Experity.SprintDashboard.API.Authentication.Encryption
{
    public class EncryptionConfigLoader : ConfigurationLoaderBase
    {
        public EncryptionConfigLoader(IConfiguration config) : base(config) { }

        public EncryptionConfig LoadEncryptionConfig()
        {
            var encryptionSettings = new EncryptionConfig
            {
                PrivateKeyFileName = base.GetSingleEnvVariable(EncryptionKeys.EncryptionPrivateKeyFileName),
                PrivateKeyFilePath = base.GetSingleEnvVariable(EncryptionKeys.EncryptionPrivateKeyFilePath),
                PublicKeyFileName = base.GetSingleEnvVariable(EncryptionKeys.EncryptionPublicKeyFileName),
                PublicKeyFilePath = base.GetSingleEnvVariable(EncryptionKeys.EncryptionPublicKeyFilePath)
            };

            return encryptionSettings;
        }
    }
}
