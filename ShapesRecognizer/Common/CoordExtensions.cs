using System;

namespace ShapesRecognizer.Common
{
    public static class CoordExtensions
    {
        public static double GetDistanceTo(this Coord a, Coord b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        
        public static bool IsOnSegment(this Coord q, Coord p, Coord r)
        {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }
        
    }
}