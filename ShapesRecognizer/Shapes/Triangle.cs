using System;
using ShapesRecognizer.Common;

namespace ShapesRecognizer.Shapes
{
    public class Triangle : Shape
    {
        public Coord A { get; init; }
        public Coord B { get; init; }
        public Coord C { get; init; }

        public Triangle(Coord a, Coord b, Coord c)
        {
            (A, B, C) = (a, b, c);
        }

        public override double GetArea()
        {
            var a = B.GetDistanceTo(C);
            var b = A.GetDistanceTo(C);
            var c = A.GetDistanceTo(B);

            var p = (a + b + c) / 2;

            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}