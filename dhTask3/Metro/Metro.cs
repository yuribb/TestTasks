using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Metro
{
    public class Metro : IMetro
    {
        public string Name { get; set; }
        public IList<IStation> Stations { get; set; }
        public IList<ILine> Lines { get; set; }

        public Metro(string name = null, IList<ILine> lines = null, IList<IStation> stations = null)
        {
            Name = name;
            Lines = lines;
            Stations = stations;
        }

        public void Add(IStation station)
        {
            if (Stations == null) Stations = new List<IStation>();
            if (station != null && !Stations.Contains(station)) Stations.Add(station);
        }

        public void Add(ILine line)
        {
            if (Lines == null) Lines = new List<ILine>();
            if (line != null && !Lines.Contains(line)) Lines.Add(line);
        }

        public IStation GetStationById(int id)
        {
            return Stations.FirstOrDefault(s => s.Id == id);
        }

        public IStation GetStationByName(string name)
        {
            if (Stations == null || !Stations.Any()) return null;
            return Stations.FirstOrDefault(x => x.Name == name);
        }

        public IRoute GetRoute(IStation A, IStation B)
        {
            if (A == B) return new Route(new List<IStation>() { A });

            IDictionary<int, IDictionary<int, int>> vertices = new Dictionary<int, IDictionary<int, int>>();

            foreach (Station station in Stations)
            {
                vertices[station.Id] = station.GraphStation;
            }

            IDictionary<int, int> previous = new Dictionary<int, int>();
            IDictionary<int, int> distances = new Dictionary<int, int>();
            List<int> nodes = new List<int>();

            IList<IStation> path = null;

            foreach (KeyValuePair<int, IDictionary<int, int>> vertex in vertices)
            {
                if (vertex.Key == B.Id)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                int smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == A.Id)
                {
                    path = new List<IStation>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(GetStationById(smallest));
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (KeyValuePair<int, int> neighbor in vertices[smallest])
                {
                    int alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            path.Add(B);
            return new Route(path);
        }

        public string GetNameById(int id)
        {
            return GetStationById(id)?.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}