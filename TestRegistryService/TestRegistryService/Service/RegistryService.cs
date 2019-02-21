using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestRegistryService.Core;

namespace TestRegistryService.Service
{
    public class RegistryService : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Logger.Information("Starting registry service");
            bool isAdmin = RegeditEditor.IsAdmin();
            StartTask(isAdmin);

            _timer = new Timer(
                (e) => OnTimer(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void StartTask(bool isAdmin)
        {
            if (!isAdmin)
            {
                Log.Logger.Information("Service not runned with admin rights. Can't change HKLM Key");
            }
            else
            {
                Log.Logger.Information("Creating registry key");
                string key = RegeditEditor.AddRegistryKey("CompanyName", "ProductName", "URL", "localhost");
                Log.Logger.Information($"Registry key '{key}' created");

                string userName = $"{Environment.UserDomainName}\\{Environment.UserName}";
                Log.Logger.Information($"Setting read permissions for key '{key}' to user '{userName}'");
                bool isPermissionSeted = RegeditEditor.ChangePermissionToUser(userName, key);
                if (isPermissionSeted)
                {
                    Log.Logger.Information("Permissions seted");
                }
                else
                {
                    Log.Logger.Information($"Unable to change permissions for the key '{key}'");
                }
            }
        }

        public void OnTimer()
        {
            Thread.Sleep(300);
            Log.Logger.Information("Registry service working");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            Log.Logger.Information("Registry service Stopped");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}