using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Core.Core;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Core
{
    public class RegistryService : IHostedService, IDisposable
    {
        private bool disposing = false;

        private Timer Timer { get; set; }
        private IApplicationLifetime ApplicationLifeTime { get; set; }
        private ILogger<RegistryService> Log { get; set; }
        IHostingEnvironment HostingEnvironment { get; set; }
        private IConfiguration Configuration { get; set; }

        public RegistryService(IConfiguration configuration, IHostingEnvironment environment, ILogger<RegistryService> log, IApplicationLifetime applicationLifeTime)
        {
            Configuration = configuration;
            ApplicationLifeTime = applicationLifeTime;
            Log = log;
            HostingEnvironment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.LogDebug("StartAsync method called.");

            ApplicationLifeTime.ApplicationStarted.Register(OnStarted);
            ApplicationLifeTime.ApplicationStopping.Register(OnStopping);
            ApplicationLifeTime.ApplicationStopped.Register(OnStopped);

            Log.LogInformation("Start service");


            Timer = new Timer(
                (e) => OnTimer(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public void OnTimer()
        {
            this.Log.LogDebug("OnTimer method called.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            this.Log.LogDebug("OnStarted method called.");

            bool isAdmin = RegeditEditor.IsAdmin();
            StartTask(isAdmin);
        }

        private void StartTask(bool isAdmin)
        {
            if (!isAdmin)
            {
                Log.LogInformation("Service not runned with admin rights. Can't change HKLM Key");
            }
            else
            {
                Log.LogInformation("Creating registry key");
                string key = RegeditEditor.AddRegistryKey("CompanyName", "ProductName", "URL", "localhost");
                Log.LogInformation($"Registry key '{key}' created");

                string userName = $"{Environment.UserDomainName}\\{Environment.UserName}";

                Log.LogInformation($"Setting read permissions for key '{key}' to user '{userName}'");
                bool isPermissionSeted = RegeditEditor.ChangePermissionToUser(userName, key);
                if (isPermissionSeted)
                {
                    Log.LogInformation("Permissions seted");
                }
                else
                {
                    Log.LogWarning($"Unable to change permissions for the key '{key}'");
                }
            }
        }

        private void OnStopping()
        {
            this.Log.LogInformation("OnStopping method called.");
        }

        private void OnStopped()
        {
            this.Log.LogInformation("OnStopped method called.");
        }

        public void Dispose()
        {
            if (!disposing)
            {
                Timer?.Dispose();
                disposing = true;
            }
        }
    }
}