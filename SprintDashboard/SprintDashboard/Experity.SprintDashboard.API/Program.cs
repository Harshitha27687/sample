using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Experity.SprintDashboard.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = ConfigHelper.ConfigureFromAppSettings();
            CreateWebHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseConfiguration(config)
                   .ConfigureLogging((hostingContext, logging) =>
                   {
                       logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   })
                   .UseNLog()
                   .UseStartup<Startup>();
    }
}
