using Backup.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalBackupPolicy
{
    public class BackupPolicy : IBackupPolicy
    {
        private class ErrorCodes
        {
            public const string DATE_LIST_NOT_INITIALIZED = "Date list not initialized";
        }

        private object SyncRoot = new object();
        public int BackupPerDays { get; set; }

        List<DateTime> DateList { get; set; }

        private int backupDelta;
        private int BackupDelta
        {
            get
            {
                if (backupDelta == 0)
                {
                    backupDelta = (24 / BackupPerDays) * 60;
                }
                return backupDelta;
            }
        }

        public DateTime NextBackupDate => DateList.First();

        public BackupPolicy(int backupPerDays)
        {
            if (backupPerDays <= 0 || backupPerDays > 24) throw new ArgumentException("Backup per Days value must be > 0 and <= 24", nameof(backupPerDays));
            BackupPerDays = backupPerDays;
        }

        public void SetBackupDates(DateTime startDate)
        {
            lock (SyncRoot)
            {
                DateList = new List<DateTime> { startDate };
            }

            for(int i = 1; i < BackupPerDays; i++)
            {
                AddNextDate();
            }
        }

        private void AddNextDate()
        {
            DateTime nextDate = DateList.Last().AddMinutes(BackupDelta);
            lock (SyncRoot)
            {
                DateList.Add(nextDate);
            }
        }

        public void BumpDate()
        {
            if (DateList == null || !DateList.Any())
            {
                throw new Exception(ErrorCodes.DATE_LIST_NOT_INITIALIZED);
            }

            lock(SyncRoot)
            {
                DateList.RemoveAt(0);
            }
            AddNextDate();
        }
    }
}