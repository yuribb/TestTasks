using Backup.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LocalRetention
{
    public class Retention : IRetention
    {
        public IRetentionPolicy RetentionPolicy { get; set; }
        public IStorage Storage { get; set; }

        private AutoResetEvent EventHandler { get; set; }

        public Retention(IRetentionPolicy retentionPolicy, IStorage storage, AutoResetEvent eventHandler)
        {
            RetentionPolicy = retentionPolicy ?? throw new ArgumentNullException(nameof(retentionPolicy));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this.EventHandler = eventHandler;
        }

        public bool KeepAndClearBackups()
        {
            var minMaxDate = RetentionPolicy.MinMaxDate;

            List<KeyValuePair<DateTime, string>> backupPaths = Storage.GetBackupPaths(minMaxDate.MinDate, minMaxDate.MaxDate);
            if (!backupPaths.Any())
            {
                return true;
            }

            while (backupPaths.Count > RetentionPolicy.NumOfCopies)
            {
                var backupPath = backupPaths.First();

                if (this.EventHandler != null) this.EventHandler.WaitOne();

                Storage.DeleteBackup(backupPath.Value);
                backupPaths.Remove(backupPath);

                EventHandler.Set();
            }

            return true;
        }
    }
}