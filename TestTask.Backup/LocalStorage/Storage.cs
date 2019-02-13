using Backup.Core.Interfaces;
using Backup.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace LocalStorage
{
    public class Storage : IStorage
    {


        private object SyncRoot = new object();
        private class ErrorCodes
        {
            public const string FOLDER_NOT_EXISTS_FORMAT = "Folder '{0}' not exists";
            public const string SOURCES_ARE_SAME_FORMAT = "Source path '{0}' is equal to destination path";
        }

        /// <summary>
        /// Constructor for backup service
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public Storage(string sourcePath, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath)) throw new ArgumentNullException(nameof(sourcePath));
            if (string.IsNullOrWhiteSpace(destinationPath)) throw new ArgumentNullException(nameof(destinationPath));

            if (sourcePath == destinationPath) throw new ArgumentException(string.Format(ErrorCodes.SOURCES_ARE_SAME_FORMAT, sourcePath), nameof(sourcePath));

            if (!Directory.Exists(sourcePath)) throw new ArgumentException(string.Format(ErrorCodes.FOLDER_NOT_EXISTS_FORMAT, sourcePath), nameof(sourcePath));
            if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);

            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            Mode = StorageMode.Backup;
        }

        /// <summary>
        /// Constructor for Retention service
        /// </summary>
        /// <param name="destinationPath"></param>
        public Storage(string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(destinationPath)) throw new ArgumentNullException(nameof(destinationPath));
            if (!Directory.Exists(destinationPath)) throw new DirectoryNotFoundException(destinationPath);

            DestinationPath = destinationPath;
            Mode = StorageMode.Retention;
        }

        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        public StorageMode Mode { get; }

        public bool Copy(string backupPath)
        {
            if (!Directory.Exists(SourcePath)) throw new ArgumentException(string.Format(ErrorCodes.FOLDER_NOT_EXISTS_FORMAT), nameof(SourcePath));
            if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

            CopyInternal(SourcePath, backupPath);
            return true;
        }

        private void CopyInternal(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(sourceDir)) return;

            if (!Directory.Exists(targetDir))
            {
                lock (SyncRoot)
                {
                    Directory.CreateDirectory(targetDir);
                }
            }

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }

            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                CopyInternal(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }

        public string GetBackupPath()
        {
            string tempBackupFolderName = Guid.NewGuid().ToString("N").ToUpper();
            string tempBackupFolderPath = Path.Combine(DestinationPath, tempBackupFolderName);

            if (!Directory.Exists(tempBackupFolderPath))
            {
                Directory.CreateDirectory(tempBackupFolderPath);
            }
            return tempBackupFolderPath;
        }

        public string Stamp(string backupPath, DateTime backupDate)
        {
            if (!Directory.Exists(backupPath)) throw new ArgumentException(string.Format(ErrorCodes.FOLDER_NOT_EXISTS_FORMAT, backupPath), nameof(backupPath));
            string stampedPath = Path.Combine(DestinationPath, backupDate.ToString("yyyy-MM-dd-HH-mm"));
            lock (SyncRoot)
            {
                if (Directory.Exists(stampedPath))
                {
                    Directory.Delete(stampedPath);
                }
                Directory.Move(backupPath, stampedPath);
            }
            return stampedPath;
        }

        public override string ToString()
        {
            return $"'{SourcePath ?? Mode.ToString()}', '{DestinationPath}'";
        }

        public List<KeyValuePair<DateTime, string>> GetBackupPaths(DateTime minDate, DateTime maxDate)
        {
            DirectoryInfo di = new DirectoryInfo(DestinationPath);
            var backupPaths = di.GetDirectories();

            List<KeyValuePair<DateTime, string>> result = new List<KeyValuePair<DateTime, string>>();

            foreach (DirectoryInfo backupPath in backupPaths)
            {
                string backupName = backupPath.Name;

                if (IsNeedToRetention(backupPath.Name, minDate, maxDate, out DateTime backupDate))
                {
                    result.Add(new KeyValuePair<DateTime, string>(backupDate, backupPath.FullName));
                }
            }
            return result;
        }

        /// <summary>
        /// Check backup for retention
        /// </summary>
        /// <param name="backupName"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <param name="backupDate"></param>
        /// <returns></returns>
        private bool IsNeedToRetention(string backupName, DateTime minDate, DateTime maxDate, out DateTime backupDate)
        {
            //Also there we can use RegEx
            backupDate = DateTime.MinValue;
            var arr = backupName.Split('-');
            if (arr.Length < 5) return false;
            if (arr[0].Length != 4) return false;
            if (arr[1].Length != 2) return false;
            if (arr[2].Length != 2) return false;
            if (arr[3].Length != 2) return false;
            if (arr[4].Length != 2) return false;

            if (!int.TryParse(arr[0], out int year)) return false;
            if (!int.TryParse(arr[1], out int months)) return false;
            if (!int.TryParse(arr[2], out int day)) return false;
            if (!int.TryParse(arr[3], out int hour)) return false;
            if (!int.TryParse(arr[4], out int minute)) return false;

            backupDate = new DateTime(year, months, day, hour, minute, 0);

            if (backupDate.Date >= minDate.Date && backupDate.Date <= maxDate.Date)
            {
                return true;
            }
            return false;
        }

        public void DeleteBackup(string backupPath)
        {
            if (!Directory.Exists(backupPath))
            {
                return;
            }

            lock(SyncRoot)
            {
                Directory.Delete(backupPath, true);
            }
        }
    }
}