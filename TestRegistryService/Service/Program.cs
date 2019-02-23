using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.Core;
using Service.Core.BaseService;
using Service.Core.BaseService.Extensions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                 .ConfigureHostConfiguration(configHost =>
                 {
                     configHost.SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory);
                     configHost.AddCommandLine(args);
                 })
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     
                     configApp.SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory);
                     configApp.AddJsonFile($"appsettings.json", true);
                     configApp.AddCommandLine(args);
                 })
                .UseStartup<Startup>()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                })

                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddSerilog(new LoggerConfiguration()
                              .ReadFrom.Configuration(hostContext.Configuration)
                              .WriteTo.File($"{System.AppDomain.CurrentDomain.BaseDirectory}\\service.log")
                              .CreateLogger());
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                });

            bool isConsole = args.Contains("-console");


            if (isConsole)
            {
                await builder.RunConsoleAsync();
                
            }
            else
            {
                await builder.RunAsServiceAsync();
            }
        }
    }
}