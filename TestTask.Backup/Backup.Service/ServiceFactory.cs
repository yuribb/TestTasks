using Backup.Core.Interfaces;
using Backup.Core.Model;
using System;

namespace Backup.Service
{
    public class ServiceFactory : IBackupServiceFactory
    {
        private int BackupPerDays { get; }
        private string SourcePath { get; }
        private string DestinationPath { get; }

        public static ServiceFactory CreateFactory(Configuration configuration)
        {
            ServiceFactory factory = new ServiceFactory(configuration.BackupPerDays, configuration.SourcePath, configuration.DestinationPath);
            factory.BackupPolicy.SetBackupDates(DateTime.Now);
            return factory;
        }

        public ServiceFactory(int backupPerDays, string sourcePath, string destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            BackupPerDays = backupPerDays;
        }

        public static Object SyncRoot = new Object();

        private volatile IStorage _storage;
        public IStorage Storage
        {
            get
            {
                if (_storage == null)
                {
                    lock(SyncRoot)
                    {
                        _storage = new LocalStorage.Storage(SourcePath, DestinationPath);
                    }
                }
                return _storage;
            }
        }

        private volatile IBackup _backup;
        public IBackup Backup
        {
            get
            {
                if (_backup == null)
                {
                    lock(SyncRoot)
                    {
                        _backup = new LocalBackup.Backup(BackupPolicy, Storage);
                    }
                }
                return _backup;
            }
        }

        private volatile IBackupPolicy _backupPolicy;

        public IBackupPolicy BackupPolicy
        {
            get
            {
                if (_backupPolicy == null)
                {
                    lock(SyncRoot)
                    {
                        _backupPolicy = new LocalBackupPolicy.BackupPolicy(BackupPerDays);
                    }
                }
                return _backupPolicy;
            }
        }
    }
}