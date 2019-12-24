using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Experity.SprintDashboard.API.Authentication.Encryption;
using Experity.SprintDashboard.API.Configuration.Models;

namespace Experity.SprintDashboard.API.Authentication
{
    public class ApplicationAuthService : IApplicationAuthService
    {
        private readonly IAuthHttpClient _authorizationHttpClient;
        private readonly IEncryptionService _encryptionService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ApplicationAuthService> _logger;


        public ApplicationAuthService(AppSettings appSettings, IAuthHttpClient authClient, EncryptionConfig encryptionSettings, ILogger<ApplicationAuthService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _authorizationHttpClient = authClient;
            _encryptionService = new EncryptionService(encryptionSettings);
        }

        public async Task<AuthenticationResult> Authenticate(HttpRequest incomingRequest)
        {
            _logger.LogTrace("ApplicationAuthService::Authenticate() started");
            var tokenHeaders = incomingRequest.Headers["Authorization"];
            if (tokenHeaders.Count == 0)
            {
                return AuthenticationResult.InvalidKey;
            }

            string token = "";
            foreach (var header in tokenHeaders)
            {
                if (header.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    token = tokenHeaders[0].Replace("Bearer ", string.Empty);
                }
            }

            _logger.LogDebug($"token: {token}");
            var authResult = await ValidateToken(token);

            _logger.LogTrace("ApplicationAuthService::Authenticate() ended");
            return authResult;
        }

        public async Task<AuthenticationResult> ValidateToken(string inboundToken)
        {
            try
            {
                _logger.LogTrace("ApplicationAuthService::ValidatingToken started");
                var sw2 = new Stopwatch();
                sw2.Start();
                var appCredentials = new ApplicationCredentials(_appSettings.ApplicationName, _appSettings.Secret);
                var encryptedAppCredentials = EncryptPayload(appCredentials);
                sw2.Stop();
                Debug.WriteLine($"encryptedAppCredentials took: {sw2.ElapsedMilliseconds}");

                var sw = new Stopwatch();
                sw.Start();
                var result = await _authorizationHttpClient.ValidateToken(encryptedAppCredentials, inboundToken);
                sw.Stop();
                Debug.WriteLine($"ValidateToken call took: {sw.ElapsedMilliseconds}");

                if (result != AuthenticationResult.Success)
                {
                    _logger.LogWarning($"Attempt to authenticate {appCredentials?.Name} resulted in {result}.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating.");
                return AuthenticationResult.Unknown;
            }
        }

        private string EncryptPayload(ApplicationCredentials decryptedAppCredentials)
        {
            var jsonSerializedObject = JsonConvert.SerializeObject(decryptedAppCredentials);
            var encryptedPayload = _encryptionService.Encrypt(jsonSerializedObject);

            return Convert.ToBase64String(encryptedPayload);
        }
    }
}
