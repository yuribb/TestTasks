using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestRegistryService.BaseService;
using TestRegistryService.Service;

namespace TestRegistryService
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("service.log").CreateLogger();
            bool isService = !(Debugger.IsAttached || args.Contains("-console"));

            try
            {
                Log.Logger.Debug("Creating service");
                IHostBuilder builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<RegistryService>();
                });

                if (isService)
                {
                    Log.Logger.Information("Starting service");
                    await builder.RunAsServiceAsync();
                }
                else
                {
                    Console.WriteLine("Start console app");
                    await builder.RunConsoleAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"{ex.Message}\r\n{ex.StackTrace}");
            }
        }
    }
}