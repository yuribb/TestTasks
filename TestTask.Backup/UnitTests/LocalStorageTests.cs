using System;
using System.Linq;
using Backup.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class LocalStorageTests
    {
        public LocalStorage.Storage Storage { get; set; }

        [TestMethod]
        public void Test_Copy()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string fileName = Guid.NewGuid().ToString("N");
            string filePath = System.IO.Path.Combine(sourcePath, fileName);
            System.IO.File.WriteAllText(filePath, sourcePath);

            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            Storage.Copy(destinationPath);

            bool isFileExists = System.IO.File.Exists(System.IO.Path.Combine(destinationPath, fileName));

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.IsTrue(isFileExists);
        }

        [TestMethod]
        public void Test_GetBackupPath()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            string backupPath = Storage.GetBackupPath();

            bool isDirectoryExists = System.IO.Directory.Exists(backupPath);

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.IsTrue(isDirectoryExists);
        }

        [TestMethod]
        public void Test_Stamp()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            string backupPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(backupPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            string stampedPath = Storage.Stamp(backupPath, DateTime.Now);

            bool isDirectoryExists = System.IO.Directory.Exists(stampedPath);

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.IsTrue(isDirectoryExists);
        }

        [TestMethod]
        public void Test_GetBackupPaths()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            string backupPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(backupPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            string stampedPath = Storage.Stamp(backupPath, DateTime.Now);

            var paths = Storage.GetBackupPaths(DateTime.Now.AddDays(-1), DateTime.Now);

            bool isDirectoryExists = (paths.FirstOrDefault(p => p.Value == stampedPath).Value != null);

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.IsTrue(isDirectoryExists);
        }

        [TestMethod]
        public void Test_DeleteBackup()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            string backupPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(backupPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            Storage.DeleteBackup(backupPath);

            bool isDirectoryExists = System.IO.Directory.Exists(backupPath);

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.IsFalse(isDirectoryExists);
        }

        [TestMethod]
        public void Test_ModeBackup()
        {
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(sourcePath);
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            Storage = new LocalStorage.Storage(sourcePath, destinationPath);

            System.IO.Directory.Delete(sourcePath, true);
            System.IO.Directory.Delete(destinationPath, true);

            Assert.AreEqual(StorageMode.Backup, Storage.Mode);
        }

        [TestMethod]
        public void Test_ModeRetention()
        {
            string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(destinationPath);

            Storage = new LocalStorage.Storage(destinationPath);

            System.IO.Directory.Delete(destinationPath, true);

            Assert.AreEqual(StorageMode.Retention, Storage.Mode);
        }
    }
}
