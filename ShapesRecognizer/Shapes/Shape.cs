using System.Drawing;

namespace ShapesRecognizer.Shapes
{
    public abstract class Shape
    {
        private static int _id = 1;
        public int Id { get; }

        public Color Color { get; set; } = Color.White;

        public Color OutLine { get; set; } = Color.Black;

        protected Shape()
        {
            Id = _id++;
        }

        public void ResetColor()
        {
            Color = Color.White;
            OutLine = Color.Black;
            
        }
        
        public abstract double GetArea();
    }
}