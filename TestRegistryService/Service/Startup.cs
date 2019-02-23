using Microsoft.Extensions.DependencyInjection;
using Service.Core.Interfaces;

namespace Service.Core.BaseService
{
    public class Startup : IStartup
    {
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<RegistryService>();

            return services;
        }
    }
}