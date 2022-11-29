using ShapesRecognizer.Common;

namespace ShapesRecognizer.Shapes
{
    public class Rectangle : Shape
    {
        public Coord TopLeft { get; init; }
        public uint Width { get; init; }
        public uint Length { get; init; }

        public Rectangle(Coord topLeft, uint width, uint length)
        {
            (TopLeft, Width, Length) = (topLeft, width, length);
        }
        public override double GetArea()
        {
            return Width * Length;
        }
    }
}