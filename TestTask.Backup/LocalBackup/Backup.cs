using Backup.Core.Interfaces;
using System;

namespace LocalBackup
{
    public class Backup : IBackup
    {
        public IStorage Storage { get; set; }
        public IBackupPolicy BackupPolicy { get; set; }

        bool IBackup.Backup()
        {
            DateTime backupDate = BackupPolicy.NextBackupDate;
            if (DateTime.Now < backupDate)
            {
                return false;
            }
            BackupPolicy.BumpDate();

            var backupPath = Storage.GetBackupPath();
            bool isSuccess = Storage.Copy(backupPath);
            if (isSuccess)
            {
                Storage.Stamp(backupPath, backupDate);
                return true;
            }
            return false;
        }

        public Backup(IBackupPolicy backupPolicy, IStorage storage)
        {
            BackupPolicy = backupPolicy ?? throw new ArgumentNullException(nameof(backupPolicy));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }
    }
}