using System;
using ShapesRecognizer.Common;

namespace ShapesRecognizer.Shapes
{
    public class Circle : Shape
    {
        public Coord Center { get; set; }
        public uint Radius { get; set; }

        public Circle(Coord center, uint radius)
        {
            (Center, Radius) = (center, radius);
        }
        public override double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
    }
}