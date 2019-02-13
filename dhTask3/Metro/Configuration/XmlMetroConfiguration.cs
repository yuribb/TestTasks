using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Metro.Configuration
{
    [Serializable]
    public class XmlMetroConfiguration
    {
        public string MetroName { get; set; }
        public List<LineConfiguration> Lines { get; set; }
        public List<StationConfiguration> Stations { get; set; }

        public static XmlMetroConfiguration CreateTestConfiguration()
        {
            return Test.TestMetroConfiguration.GetMoscowMetroTempConfiguration();
        }

        public static XmlMetroConfiguration CreateConfiguration(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path)) throw new FileNotFoundException($"File '{path}' not found", path);

            XmlSerializer formatter = new XmlSerializer(typeof(XmlMetroConfiguration));
            XmlMetroConfiguration configuration;
            using (Stream fs = new FileStream(path, FileMode.Open))
            {
                configuration = (XmlMetroConfiguration)formatter.Deserialize(fs);
                fs.Close();
            }
            return configuration;
        }

        public void SaveConfiguration(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            XmlSerializer formatter = new XmlSerializer(typeof(XmlMetroConfiguration));

            // получаем поток, куда будем записывать сериализованный объект
            using (Stream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
                fs.Close();
            }
        }
    }

    [Serializable]
    public class LineConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ColorId { get; set; }
    }

    [Serializable]
    public class LineStationConfiguration
    {
        public int LineId { get; set; }
        public string StationName { get; set; }
    }

    [Serializable]
    public class StationConfiguration
    {
        public int Id { get; set; }
        public List<LineStationConfiguration> LineStations { get; set; }
        public List<int> NeighboursStationIds { get; set; }
    }
}