using System.IO;
using Api.AspNetInfra;
using Api.Utility;
using Microsoft.AspNetCore.Hosting;

namespace Api
{
    class Program
    {
        private static ILogger Log;

        static void Main(string[] args)
        {
            Logger.Initialize(verbose : true);
            Log = new Logger(typeof(Program));
            Log.Info("CQRS Example Starting...");

            var host = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            Log.Info("Starting host");
            host.Run();

            Log.Info("CQRS Example Exiting...");
        }
    }
}
