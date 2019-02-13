using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Metro
{
    [Serializable]
    public class Station : IStation
    {
        public int Id { get; set; }
        public string Name => string.Join(" - ", LineStations.Select(ls => ls.StationName).Distinct());

        [XmlIgnore]
        public IDictionary<int, int> GraphStation
        {
            get
            {
                IDictionary<int, int> graphs = new Dictionary<int, int>();
                foreach(IStation station in NeighborStation)
                {
                    graphs.Add(station.Id, 1);
                }
                return graphs;
            }
        }

        public IList<ILineStation> LineStations { get; set; }

        [XmlIgnore]
        public IList<IStation> NeighborStation { get; set; }

        public Station() { }

        public Station(int id, string name = null, ILine line = null, IStation neigbor = null)
        {
            Id = id;
            AddNeighbor(neigbor);
            AddLine(name, line);
        }

        public void AddLine(string lineStationName, ILine line)
        {
            if (line == null) return;
            if (string.IsNullOrEmpty(lineStationName)) throw new ArgumentNullException(nameof(lineStationName));
            if (line == null) throw new ArgumentNullException(nameof(line));
            if (LineStations == null) LineStations = new List<ILineStation>();
            LineStations.Add(new LineStation(lineStationName, line));
        }

        public void AddNeighbor(IStation station)
        {
            if (station == null) return;
            if (NeighborStation == null) NeighborStation = new List<IStation>();
            if (!NeighborStation.Contains(station))
            {
                NeighborStation.Add(station);
                station.AddNeighbor(this);
            }
        }

        public void AddNeighbors(IEnumerable<IStation> neigbors)
        {
            if (neigbors != null && neigbors.Any())
            {
                foreach (IStation neighborStation in neigbors)
                {
                    AddNeighbor(neighborStation);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is IStation)
            {
                IStation station = obj as IStation;
                return station.Id == this.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}