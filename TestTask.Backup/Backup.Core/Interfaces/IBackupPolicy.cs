using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Core.Interfaces
{
    public interface IBackupPolicy
    {
        DateTime NextBackupDate { get; }

        int BackupPerDays { get; set; }
        void BumpDate();

        void SetBackupDates(DateTime startDate);
    }
}