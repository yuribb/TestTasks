using System;

namespace Core.Interfaces
{
    public interface ILine
    {
        int Id { get; set; }
        string Name { get; set; }
        ConsoleColor Color { get; set; }
    }
}