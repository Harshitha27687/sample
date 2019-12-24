using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Experity.SprintDashboard.API.Authentication
{
    public interface IApplicationAuthService
    {
        Task<AuthenticationResult> Authenticate(HttpRequest incomingRequest);
    }
}
