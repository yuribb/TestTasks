using Core.Interfaces;
using Factory;
using RuleLibrary.Rules;
using System.Collections.Generic;

namespace ServiceFactory
{
    public class ServiceFactory
    {
        private static IService service;

        public static IService CreateService(string watchPath, string storagePath)
        {
            if (service == null)
            {
                IStorage storage = new Storage.Storage(storagePath);
                IList<IRule> rules = new List<IRule>();
                rules.Add(new HTMLRule(new List<IOperation>() { new RuleLibrary.Operations.GetDivsCount() }));
                rules.Add(new CSSRule(new List<IOperation>() { new RuleLibrary.Operations.GetBracesCount() }));
                rules.Add(new CommonRule(new List<IOperation>() { new RuleLibrary.Operations.GetPunctuationCount() }));
                service = Service.CreateService(watchPath, storage, rules);
            }
            return service;
        }
    }
}