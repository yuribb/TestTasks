using Backup.Core.Interfaces;
using Backup.Core.Model;
using System.Linq;
using System.ServiceProcess;

namespace Retention.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (!args.Any()) return;
            bool isManual = args.Any() && args.First().ToLower() == "/console";
            string configurationPath = args[isManual ? 1 : 0];
            Configuration configuration = Configuration.CreateConfiguration(configurationPath);

            IRetentionServiceFactory serviceFactory = ServiceFactory.CreateFactory(configuration);
            ServiceBase retentionService = new RetentionService(serviceFactory);

            if (isManual)
            {
                ((RetentionService)retentionService).ManualRun();
                return;
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                retentionService
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}