using Core.Interfaces;

namespace Metro
{
    public class LineStation : ILineStation
    {
        public string StationName { get; set; }
        public ILine Line { get; set; }

        public string FullStationName => $"{StationName}-{Line.Name}";

        public LineStation(string name, ILine line)
        {
            StationName = name;
            Line = line;
        }


        public override string ToString()
        {
            return FullStationName;
        }
    }
}