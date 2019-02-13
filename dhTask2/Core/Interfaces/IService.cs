using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IService
    {
        string WatchPath { get; set; }
        IList<IRule> Rules { get; }
        IStorage Storage { get; set; }
        void AddRule(IRule rule);
        void ApplyRules(string filePath);
        void Run();
    }
}