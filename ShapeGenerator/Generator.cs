using System;
using System.Collections.Generic;
using ShapesRecognizer.Common;
using ShapesRecognizer.Shapes;

namespace ShapeGenerator
{
    public static class Generator
    {
        public static uint WindowWidth { get; private set; } = 800;
        public static uint WindowLength { get; private set; } = 600;

        private static Random _random = new Random();

        public static void SetWindowSize(uint windowWidth, uint windowLength)
        {
            WindowWidth = windowWidth;
            WindowLength = windowLength;
        }

        public static List<Shape> GenerateAllShapes(uint count, uint maxSide, uint maxRadius, bool includeLines = true)
        {
            var shapes = new List<Shape>();
            var maxShapesInARow = (uint)_random.Next(1,5);
            if (count < maxShapesInARow)
            {
                maxShapesInARow = 1;
            }
            Console.WriteLine(maxShapesInARow);
            var typeOfShape = 1;
            var shapeTypes = 4;

            for (int i = 0; i < count; i += (int)maxShapesInARow)
            {
                switch (typeOfShape)
                {
                    case 1:
                        shapes.AddRange(GenerateRectangles(maxShapesInARow, (maxSide, maxSide)));
                        break;
                    case 2:
                        shapes.AddRange(GenerateCircles(maxShapesInARow, maxRadius));
                        break;
                    case 3:
                        shapes.AddRange(GenerateTriangles(maxShapesInARow, maxSide));
                        break;
                    case 4:
                        if (includeLines)
                        {
                            shapes.AddRange(GenerateLines(maxShapesInARow, maxSide));
                        }
                        else
                        {
                            shapes.AddRange(GenerateCircles(maxShapesInARow, maxRadius));
                        }
                        break;
                }
                
                typeOfShape++;
                if (typeOfShape > shapeTypes)
                {
                    typeOfShape = 1;
                }
            }

            shapes.AddRange(GenerateRectangles(count % maxShapesInARow, (maxSide, maxSide)));

            return shapes;
        }

        public static List<Rectangle> GenerateRectangles(uint count,
            (uint maxRectWidth, uint maxRectLength) maxRectSize)
        {
            var list = new List<Rectangle>();

            for (int i = 0; i < count; i++)
            {
                var width = (uint)_random.Next(1,(int)maxRectSize.maxRectWidth);
                var length = (uint)_random.Next(1,(int)maxRectSize.maxRectLength);

                var topLeft = GenerateCoord(WindowWidth - width, WindowLength - length);
                list.Add(new Rectangle(topLeft, width, length));
            }

            return list;
        }

        public static List<Circle> GenerateCircles(uint count, uint maxRadius)
        {
            var list = new List<Circle>();

            for (int i = 0; i < count; i++)
            {
                var radius = (uint)_random.Next(1,(int)maxRadius);
                list.Add(new Circle(GenerateCoord(WindowWidth - radius, WindowLength - radius), radius));
            }

            return list;
        }

        public static List<Triangle> GenerateTriangles(uint count, uint maxSideLength)
        {
            var list = new List<Triangle>();
            var sideLength = (int) maxSideLength;
            var minValue = sideLength/3;
            
            for (int i = 0; i < count; i++)
            {
                var a = GenerateCoord(WindowWidth-maxSideLength, WindowLength-maxSideLength);
                var b = GetPointNear(a, minValue, (int)maxSideLength);
                var c = GetPointNear(a, minValue, (int)maxSideLength);

                list.Add(new Triangle(a, b, c));
            }

            return list;
        }

        private static Coord GetPointNear(Coord a, int minDelta, int maxDelta)
        {
            int deltaX;
            int deltaY;
            do
            {
                deltaX = _random.Next(minDelta, maxDelta);
                deltaY = _random.Next(minDelta, maxDelta);
                var sign1 = _random.Next(2) == 1 ? -1 : 1;
                var sign2 = _random.Next(2) == 1 ? -1 : 1;
                deltaX *= sign1;
                deltaY *= sign2;
            } while (a.X+deltaX <= 0 || a.Y+deltaY <= 0);
            
            return new Coord(a.X+deltaX, a.Y+deltaY);
        }

        public static List<Line> GenerateLines(uint count, uint maxLength)
        {
            var list = new List<Line>();

            for (int i = 0; i < count; i++)
            {
                var a = GenerateCoord(WindowWidth, WindowLength);
                var b = GenerateCoord(WindowWidth, WindowLength);

                list.Add(new Line(a,b));
            }

            return list;
        }

        private static Coord GenerateCoord(uint maxX, uint maxY)
        {
            var x = _random.Next((int)maxX);
            var y = _random.Next((int)maxY);

            return new Coord(x, y);
        }
    }
}