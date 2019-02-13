using Backup.Core.Model;
using System;
using System.Collections.Generic;

namespace Backup.Core.Interfaces
{
    public interface IStorage
    {
        StorageMode Mode { get; }
        string SourcePath { get; set; }
        string DestinationPath { get; set; }
        string GetBackupPath();
        bool Copy(string backupPath);
        string Stamp(string backupPath, DateTime backupDate);
        List<KeyValuePair<DateTime, string>> GetBackupPaths(DateTime minDate, DateTime maxDate);
        void DeleteBackup(string backupPath);
    }
}
