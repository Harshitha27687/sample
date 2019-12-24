using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Experity.SprintDashboard.API.Authentication;

namespace Experity.SprintDashboard.API.Middlewares
{
    public class ValidateTokenFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // execute any code before the action executes
            var authService = context.HttpContext.RequestServices.GetService<IApplicationAuthService>();
            var sw = new Stopwatch();
            sw.Start();
            // call the auth api to validate the token
            var validateResult = await authService.Authenticate(context.HttpContext.Request);
            sw.Stop();
            Debug.WriteLine($"Full authenticate call from ValidateTokenFilter took: {sw.ElapsedMilliseconds}");

            if (validateResult != AuthenticationResult.Success)
            {
                context.Result = new ContentResult
                {
                    Content = "Access Denied: Application is not authenticated.",
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else
            {
                await next();
            }
        }
    }
}
