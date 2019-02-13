using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILineStation
    {
        string StationName { get; set; }
        string FullStationName { get; }
        ILine Line { get; set; }
    }
}