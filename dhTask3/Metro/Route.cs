using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metro
{
    public class Route : IRoute
    {
        public IList<IStation> Stations { get; set; }

        public Route(IList<IStation> stations)
        {
            Stations = stations;
        }

        public string GetRouteList()
        {
            if (!Stations.Any()) return null;
            if (Stations.Count == 1) return Stations.First().Name;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Stations.Count; i++)
            {
                if (i == 0 || i == Stations.Count - 1)
                {
                    if (Stations[i].LineStations.Count == 1)
                    {
                        sb.Append(Stations[i].Name);
                    }
                    else
                    {
                        int secondStation = i == 0 ? i + 1 : i - 1;
                        if (secondStation >= Stations.Count) throw new IndexOutOfRangeException("Can't get next station");
                        ILineStation lineStation = GetLineStationByRoute(Stations[i], Stations[secondStation]);
                        if (lineStation != null)
                        {
                            sb.Append(lineStation.StationName);
                        }
                    }
                }
                else
                {
                    if (Stations[i].LineStations.Count == 1)
                    {
                        sb.Append(Stations[i].Name);
                    }
                    else
                    {
                        sb.Append(GetTransferString(Stations[i - 1], Stations[i], Stations[i + 1]));
                    }
                }

                if (i < Stations.Count - 1)
                {
                    sb.Append(" => ");
                }
            }

            return sb.ToString();
        }

        public ILineStation GetLineStationByRoute(IStation station, IStation secondStation)
        {
            ILineStation lineStation = null;
            foreach (ILineStation ls in secondStation.LineStations)
            {
                lineStation = station.LineStations.FirstOrDefault(x => x.Line.Id == ls.Line.Id);
                if (lineStation != null)
                {
                    break;
                }
            }
            return lineStation;
        }

        public string GetTransferString(IStation previousStation, IStation station, IStation nextStation)
        {
            ILineStation firstlineStation = GetLineStationByRoute(station, previousStation);
            ILineStation secondLineStation = GetLineStationByRoute(station, nextStation);

            if (firstlineStation.Line.Id == secondLineStation.Line.Id)
            {
                return station.LineStations.FirstOrDefault(ls => ls.Line.Id == firstlineStation.Line.Id).StationName;
            }

            if (firstlineStation.StationName != secondLineStation.StationName)
            {
                return $"{firstlineStation.StationName} пересадка на {secondLineStation.StationName}";
            }
            else
            {
                return $"{firstlineStation.FullStationName} пересадка на {secondLineStation.FullStationName}";
            }
        }
    }
}