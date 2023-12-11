using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using DuaTaxi.Common.Logging;
using DuaTaxi.Common.Metrics;
using DuaTaxi.Common.Mvc;
using DuaTaxi.Common.Vault;

namespace DuaTaxi.Services.Signalr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseLogging()
                .UseVault()
                .UseLockbox()
                .UseAppMetrics();
    }
}