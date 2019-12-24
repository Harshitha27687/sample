using System.Threading.Tasks;

namespace Experity.SprintDashboard.API.Authentication
{
    public interface IAuthHttpClient
    {
        Task<AuthenticationResult> ValidateToken(string appCredentials, string tokenToValidate);
    }
}
