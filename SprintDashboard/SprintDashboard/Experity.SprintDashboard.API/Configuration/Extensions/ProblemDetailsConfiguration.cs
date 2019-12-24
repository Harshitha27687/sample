using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Experity.SprintDashboard.API.Configuration.Extensions
{
    public static class ProblemDetailsConfigurer
    {
        public static void AddProblemDetails(this IServiceCollection serviceCollection, bool isDevelop)
        {

            serviceCollection.AddProblemDetails(config =>
            {
                config.IncludeExceptionDetails = httpContext => isDevelop;

                config.ShouldLogUnhandledException = (httpContext, ex, problemDetails) =>
                {
                    //Don't log argument exceptions as unhandled.  
                    //Consider creating our own exception types for our guard clauses so we do not have to blindly handle all of these.
                    var isArgumentException = (ex is ArgumentException || ex is ArgumentNullException || ex is ArgumentOutOfRangeException);
                    return !isArgumentException;
                };

                config.Map<ArgumentException>((ex) => new ProblemDetails()
                {
                    Title = $"{ex.ParamName} is invalid.",
                    Type = "Invalid argument",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                });

                config.Map<ArgumentNullException>((ex) => new ProblemDetails()
                {
                    Title = $"{ex.ParamName} is invalid.",
                    Type = "Empty or Blank",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{ex.ParamName} cannot be null."
                });

                config.Map<ArgumentOutOfRangeException>((ex) => new ProblemDetails()
                {
                    Title = $"{ex.ParamName} is invalid.",
                    Type = "Out of range",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{ex.ActualValue} is not valid for {ex.ParamName}.{Environment.NewLine}{ex.Message}"
                });
            });
        }
    }
}
