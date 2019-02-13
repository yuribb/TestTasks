using Core;
using Core.Interfaces;
using System.Threading;

namespace dhTask2
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Info("Program started");
            IService service = ServiceFactory.ServiceFactory.CreateService(args[0], args[1]);
            service.Run();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}