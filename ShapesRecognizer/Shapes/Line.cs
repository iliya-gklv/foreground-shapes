using ShapesRecognizer.Common;

namespace ShapesRecognizer.Shapes
{
    public class Line : Shape
    {
        public Coord A { get; set; }
        public Coord B { get; set; }
        
        public Line(Coord a, Coord b) => (A, B) = (a, b);
        
        public override double GetArea()
        {
            return A.GetDistanceTo(B);
        }
    }
}