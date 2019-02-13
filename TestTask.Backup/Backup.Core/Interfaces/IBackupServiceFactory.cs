using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Core.Interfaces
{
    public interface IBackupServiceFactory
    {
        IStorage Storage { get; }
        IBackup Backup { get; }
        IBackupPolicy BackupPolicy { get; }
    }
}
