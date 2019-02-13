using Core.Interfaces;
using Metro.Configuration;
using System.Linq;

namespace MetroFactory
{
    public class Factory
    {
        public IMetro Metro { get; set; }
        public IService Service { get; set; }
        public XmlMetroConfiguration Configuration { get; set; }

        public static Factory CreateFactory(string configurationPath = null)
        {
            Factory factory = new Factory();
            if (configurationPath != null)
            {
                factory.Metro = factory.CreateMetro(configurationPath);
            }
            else
            {
                factory.Configuration = XmlMetroConfiguration.CreateTestConfiguration();
                factory.Metro = MetroConfigurationAdapter.CreateMetro(factory.Configuration);
            }
            
            factory.Service = new MetroService.Service();
            return factory;
        }

        public IMetro CreateMetro(string configurationPath)
        {
            Configuration = XmlMetroConfiguration.CreateConfiguration(configurationPath);
            Metro = MetroConfigurationAdapter.CreateMetro(Configuration);
            return Metro;
        }

        public void SaveMetro(string path)
        {
            Configuration.SaveConfiguration(path);
        }

        public string GetRoute(IStation A, IStation B)
        {
            return Service.GetRoute(Metro, A, B).GetRouteList();
        }

        public string[] GetStationArray()
        {
            return Metro.Stations.Select(x => x.Name).ToArray();
        }
    }
}