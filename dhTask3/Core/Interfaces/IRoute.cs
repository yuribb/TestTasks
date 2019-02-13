using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IRoute
    {
        IList<IStation> Stations { get; set; }
        string GetRouteList();
    }
}