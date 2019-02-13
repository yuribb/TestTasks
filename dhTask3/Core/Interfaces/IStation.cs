using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IStation
    {
        int Id { get; set; }
        string Name { get; }

        IList<ILineStation> LineStations {get;set;}

        IDictionary<int, int> GraphStation { get; }

        void AddLine(string lineStationName, ILine line);
        void AddNeighbor(IStation station);
    }
}