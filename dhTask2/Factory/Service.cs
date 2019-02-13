using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Factory
{
    public class Service : IService
    {
        public static IService CreateService(string watchPath, IStorage storage, IList<IRule> rules)
        {
            IService service = new Service();
            service.WatchPath = watchPath;
            service.Storage = storage;

            foreach(IRule rule in rules)
            {
                service.Rules.Add(rule);
            }
            return service;
        }

        public string WatchPath { get; set; }

        private IList<IRule> rules;

        public IList<IRule> Rules
        {
            get
            {
                if (rules == null)
                {
                    rules = new List<IRule>();
                }
                return rules;
            }
        }

        public IStorage Storage { get; set; }

        public void Run()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(WatchPath);

            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
            Logger.Info("Service started");
            InitialApply();
        }

        private void InitialApply()
        {
            DirectoryInfo di = new DirectoryInfo(WatchPath);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            
            foreach(FileInfo file in files)
            {
                ApplyRules(file.FullName);
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.Info($"new file {e.Name}");
            ApplyRules(e.FullPath);
        }

        public void AddRule(IRule rule)
        {
            if(rule != null)
            {
                if (!Rules.Contains(rule))
                {
                    Rules.Add(rule);
                }
            }
        }

        public void ApplyRules(string filePath)
        {
            foreach(IRule rule in Rules)
            {
                if (rule.IsRuleApply(filePath))
                {
                    string result = rule.ApplyRule(filePath);
                    bool isResultSaved = Storage.SaveResult(result);

                    if (isResultSaved)
                    {
                        Logger.Info(result);
                    }
                    else
                    {
                        Logger.Warn($"Can't apply rule {rule.GetType().ToString()} to file '{filePath}'");
                    }
                    break;
                }
            }
        }
    }
}