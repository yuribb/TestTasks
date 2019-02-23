using Microsoft.Extensions.DependencyInjection;

namespace Service.Core.Interfaces
{
    public interface IStartup
    {
        IServiceCollection ConfigureServices(IServiceCollection services);
    }
}
