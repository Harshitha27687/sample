namespace Experity.SprintDashboard.API.Configuration.Models
{
    public class JwtSettingsModel
    {
        public string PublicKeyFilePath { get; set; }
        public string PublicKeyFileName { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public string JwtAppNameClaim { get; set; }
        public string JwtPermissionsClaim { get; set; }
    }
}
