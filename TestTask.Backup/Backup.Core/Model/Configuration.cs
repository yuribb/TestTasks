using Backup.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Backup.Core.Model
{
    [Serializable]
    public class Configuration
    {
        private class ErrorCodes
        {
            public const string CONFIGURATION_FILE_NOT_FOUND = "Configuration file not found";
        }

        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public int BackupPerDays { get; set; }
        public List<RetentionPolicyConfiguration> RetentionPolicies { get; set; }

        public static Configuration CreateConfiguration(string configurationPath)
        {
            if (string.IsNullOrWhiteSpace(configurationPath)) throw new ArgumentNullException(nameof(configurationPath));
            if (!File.Exists(configurationPath)) throw new FileNotFoundException(ErrorCodes.CONFIGURATION_FILE_NOT_FOUND, configurationPath);

            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            Configuration configuration;
            using (StreamReader reader = new StreamReader(configurationPath))
            {
                configuration = (Configuration)serializer.Deserialize(reader);
                reader.Close();
            }
            return configuration;
        }
    }
}