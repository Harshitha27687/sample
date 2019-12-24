using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Experity.SprintDashboard.Models
{
    // Based on RFC7807 https://tools.ietf.org/html/rfc7807
    public class ValidationErrorResponse : ProblemDetails
    {
        public ValidationErrorResponse(string title, string detail, List<ValidationError> validationErrors,
            string type = null, string instance = null)
        {
            Title = title;
            Detail = detail;
            ValidationErrors = validationErrors;
            Type = type;
            Instance = instance;
        }
        public List<ValidationError> ValidationErrors { get; set; }
    }
}
