using Core.Interfaces;
using System;

namespace Metro
{
    public class Line : ILine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }

        public Line() { }
        public Line(int id, string name, ConsoleColor color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public override string ToString()
        {
            return $"{Id}) {Name} линия";
        }

        public override bool Equals(object obj)
        {
            if (obj is Line)
            {
                Line line = obj as Line;
                return line.Id == this.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}