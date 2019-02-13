using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IMetro
    {
        string Name { get; set; }
        IList<IStation> Stations { get; set; }
        IList<ILine> Lines { get; set; }
        void Add(IStation station);
        void Add(ILine line);
        IStation GetStationById(int id);
        IStation GetStationByName(string name);
    }
}