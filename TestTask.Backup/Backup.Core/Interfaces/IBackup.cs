using System;

namespace Backup.Core.Interfaces
{
    public interface IBackup
    {
        IStorage Storage { get; set; }
        IBackupPolicy BackupPolicy { get; set; }
        bool Backup();
    }
}