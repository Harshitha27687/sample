namespace Experity.SprintDashboard.API.Authentication
{
    public class ApplicationCredentials
    {
        public string Name { get; }
        public string Secret { get; }

        public ApplicationCredentials(string name, string secret)
        {
            Name = name;
            Secret = secret;
        }
    }
}
