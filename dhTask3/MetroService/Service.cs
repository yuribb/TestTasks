using Core.Interfaces;
using Metro;
using System.Collections.Generic;

namespace MetroService
{
    public class Service : IService
    {
        public IRoute GetRoute(IMetro metro, IStation A, IStation B)
        {
            if (A == B) return new Route(new List<IStation>() { A });

            IDictionary<int, IDictionary<int, int>> vertices = new Dictionary<int, IDictionary<int, int>>();

            foreach (IStation station in metro.Stations)
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
                        path.Add(metro.GetStationById(smallest));
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
    }
}
