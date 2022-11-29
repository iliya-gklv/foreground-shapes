using System;
using System.Collections.Generic;
using NUnit.Framework;
using ShapesRecognizer.Common;
using ShapesRecognizer.Shapes;

namespace ShapesRecognizerTests
{
    public class AreaTests
    {
        [Test]
        [TestCaseSource(nameof(AreaTestData))]
        public void GetArea_Success(Shape shape, double expected)
        {
            var area = shape.GetArea();
            
            Assert.That(area, Is.EqualTo(expected).Within(5));
        }

        static IEnumerable<object[]> AreaTestData()
        {
            yield return new object[] { new Triangle(new Coord(1, 1), new Coord(1, 5), new Coord(5, 5)), 8 };
            yield return new object[] { new Triangle(new Coord(0, 0), new Coord(0, 3), new Coord(4, 0)), 6 };
            yield return new object[] { new Rectangle(new Coord(1, 1), 5, 6), 30 };
            yield return new object[] { new Circle(new Coord(100, 100), 20), Math.PI*400 };
            yield return new object[] { new Line(new Coord(100, 100), new Coord(150, 100)), 50 };
            yield return new object[] { new Line(new Coord(0, 3), new Coord(4, 0)), 5 };
        }
    }
}