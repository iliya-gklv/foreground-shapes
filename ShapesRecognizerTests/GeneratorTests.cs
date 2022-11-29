using NUnit.Framework;
using ShapeGenerator;

namespace ShapesRecognizerTests
{
    public class GeneratorTests
    {
        [Test]
        [TestCaseSource(nameof(_counts))]
        public void GenerateAllShapes_CorrectAmount(int expectedCount)
        {
            Generator.SetWindowSize(600, 600);
            var shapes = Generator.GenerateAllShapes((uint)expectedCount, 30, 20);
            var circles = Generator.GenerateCircles((uint)expectedCount, 30);
            var triangles = Generator.GenerateTriangles((uint)expectedCount, 30);
            var lines = Generator.GenerateLines((uint)expectedCount, 30);
            var rectangles = Generator.GenerateRectangles((uint)expectedCount, (40, 50));

            
            Assert.That(shapes.Count, Is.EqualTo(expectedCount));
            Assert.That(circles.Count, Is.EqualTo(expectedCount));
            Assert.That(triangles.Count, Is.EqualTo(expectedCount));
            Assert.That(lines.Count, Is.EqualTo(expectedCount));
            Assert.That(rectangles.Count, Is.EqualTo(expectedCount));
        }

        private static int[] _counts =
        {
            1, 30, 100, 0
        };
    }
}