using Microsoft.AspNetCore.Mvc;

namespace Experity.SprintDashboard.Models
{
    // Based on RFC7807 https://tools.ietf.org/html/rfc7807
    public class ErrorResponse : ProblemDetails
    {
        public ErrorResponse(string title, string detail, string type = null, string instance = null)
        {
            Title = title;
            Detail = detail;
            Type = type;
            Instance = instance;
        }
    }
}
