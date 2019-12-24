using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Experity.SprintDashboard.API.Authentication
{
    public class AuthHttpClient : IAuthHttpClient
    {
        private readonly HttpClient _client;
        private const string ValidateTokenRoute = "Token/validate";

        public AuthHttpClient(AuthApiConfig appAuthConfig)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(appAuthConfig.BaseAddress);

            if (appAuthConfig.HeaderDefinitions != null)
            {
                foreach (KeyValuePair<string, string> header in appAuthConfig.HeaderDefinitions)
                {
                    _client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            _client.DefaultRequestHeaders.TransferEncodingChunked = false;

            //Default to application/json if no header type is specified
            if (_client.DefaultRequestHeaders.Accept.Count == 0)
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<AuthenticationResult> ValidateToken(string appCredentials, string tokenToValidate)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ValidateTokenRoute);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("encryptedCredentials", appCredentials);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenToValidate);

            using (var response = await _client.SendAsync(request))
            {
                var statusCode = response.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    return AuthenticationResult.Success;
                }

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    return AuthenticationResult.Unauthorized;
                }
            }

            return AuthenticationResult.Unknown;
        }
    }
}
