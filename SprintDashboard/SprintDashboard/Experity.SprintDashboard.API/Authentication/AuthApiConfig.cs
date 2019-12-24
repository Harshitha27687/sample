using System.Collections.Generic;
using System.Text;

namespace Experity.SprintDashboard.API.Authentication
{
    public class AuthApiConfig
    {
        public string ApiName { get; set; }
        public string BaseAddress { get; set; }
        public Dictionary<string, string> HeaderDefinitions { get; set; }
        public string BaseRef { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(ApiName)}: {ApiName}");
            sb.AppendLine($"{nameof(BaseAddress)}: {BaseAddress}");
            sb.AppendLine($"{nameof(HeaderDefinitions)}: {HeaderDefinitions}");
            sb.AppendLine($"{nameof(BaseRef)}: {BaseRef}");
            return sb.ToString();
        }
    }
}
