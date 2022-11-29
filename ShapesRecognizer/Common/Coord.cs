namespace ShapesRecognizer.Common
{
    public class Coord
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Coord(int x, int y) => (X, Y) = (x,y);
        
        public Coord(Coord coordToCopy) => (X, Y) = (coordToCopy.X, coordToCopy.Y);

    }
}