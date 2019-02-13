using Core;
using Core.Interfaces;
using System;
using System.IO;

namespace Storage
{
    public class Storage : IStorage
    {
        private static object SyncRoot = new object();
        public string StorageFilePath { get; set; }

        public Storage(string storageFilePath)
        {
            StorageFilePath = storageFilePath;
        }

        public bool SaveResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            if (!File.Exists(StorageFilePath))
            {
                CreateFile(StorageFilePath);
            }
            return AppendResult(result);
        }

        private static void CreateFile(string filePath)
        {
            lock (SyncRoot)
            {
                try
                {
                    using (FileStream stream = File.Create(filePath))
                    {
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Can't create file '{filePath}'", ex);
                    throw;
                }
            }
        }

        private bool AppendResult(string result)
        {
            if (!string.IsNullOrWhiteSpace(result))
            {
                lock (SyncRoot)
                {
                    AppendText(result);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AppendText(string text)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
                File.AppendAllText(StorageFilePath, $"{Environment.NewLine}{text}");
            }
            catch(ArgumentNullException)
            {
                Logger.Error("Can't write null or empty text");
                throw;
            }
            catch(IOException ex)
            {
                Logger.Error($"Can't write text '{text}' to '{StorageFilePath}'", ex);
                throw;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public override string ToString()
        {
            return StorageFilePath ?? "Unknown";
        }
    }
}