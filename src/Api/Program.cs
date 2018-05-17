using System.IO;
using Api.AspNetInfra;
using Api.Utility;
using Microsoft.AspNetCore.Hosting;

namespace Api
{
    public class Program
    {
        public static IWebHost Host;
        private static ILogger Log;

        public static void Main(string[] args)
        {
            Logger.Initialize(verbose : true);
            Log = new Logger(typeof(Program));
            Log.Info("CQRS Example Starting...");

            Host = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            Log.Info("Starting host");
            Host.Run();

            Log.Info("CQRS Example Exiting...");
        }
    }
}
