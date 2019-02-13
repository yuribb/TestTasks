using Backup.Core.Interfaces;
using Backup.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Retention.Service
{
    public class ServiceFactory : IRetentionServiceFactory
    {
        public static Object SyncRoot = new Object();
        public static AutoResetEvent eventHandler = new AutoResetEvent(true);

        private string DestinationPath { get; }
        public IList<IRetention> Retentions { get; set; }

        private volatile IStorage _storage;
        public IStorage Storage
        {
            get
            {
                if (_storage == null)
                {
                    lock (SyncRoot)
                    {
                        _storage = new LocalStorage.Storage(DestinationPath);
                    }
                }
                return _storage;
            }
        }

        public static ServiceFactory CreateFactory(Configuration configuration)
        {
            if (configuration.RetentionPolicies == null || !configuration.RetentionPolicies.Any()) throw new ArgumentNullException(nameof(configuration.RetentionPolicies));
            if (string.IsNullOrWhiteSpace(configuration.DestinationPath)) throw new ArgumentNullException(nameof(configuration.DestinationPath));

            ServiceFactory serviceFactory = new ServiceFactory(configuration.DestinationPath);
            foreach(RetentionPolicyConfiguration retentionPolicyConfiguration in configuration.RetentionPolicies)
            {
                IRetentionPolicy retentionPolicy = new LocalRetentionPolicy.RetentionPolicy(retentionPolicyConfiguration);
                serviceFactory.AddRetention(retentionPolicy);
            }

            return serviceFactory;
        }

        public ServiceFactory(string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(destinationPath)) throw new ArgumentNullException(nameof(destinationPath));
            DestinationPath = destinationPath;
        }

        private void AddRetention(IRetentionPolicy retentionPolicy)
        {
            if (Retentions == null) Retentions = new List<IRetention>();
            IRetention retention = new LocalRetention.Retention(retentionPolicy, Storage, eventHandler);
            Retentions.Add(retention);
        }
    }
}