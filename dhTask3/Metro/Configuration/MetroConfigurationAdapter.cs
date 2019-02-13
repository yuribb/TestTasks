using Core.Interfaces;
using System;
using System.Linq;

namespace Metro.Configuration
{
    public static class MetroConfigurationAdapter
    {
        public static IMetro CreateMetro(XmlMetroConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IMetro metro = new Metro(configuration.MetroName);

            foreach(LineConfiguration line in configuration.Lines)
            {
                ILine l = new Line(line.Id, line.Name, (ConsoleColor)line.ColorId);
                metro.Add(l);
            }

            foreach (StationConfiguration station in configuration.Stations)
            {
                IStation s = new Station(station.Id);

                foreach(LineStationConfiguration ls in station.LineStations)
                {
                    ILine l = metro.Lines.FirstOrDefault(ml => ml.Id == ls.LineId);
                    if (l != null)
                    {
                        s.AddLine(ls.StationName, l);
                    }
                }
                metro.Add(s);
            }

            foreach(StationConfiguration station in configuration.Stations)
            {
                IStation s = metro.GetStationById(station.Id);
                foreach (int neighbourStationId in station.NeighboursStationIds)
                {
                    IStation ns = metro.GetStationById(neighbourStationId);
                    s.AddNeighbor(ns);
                }
            }
            return metro;
        }
    }
}