using System.Collections.Generic;
using NUnit.Framework;
using ShapesRecognizer.Common;

namespace ShapesRecognizerTests
{
    public class CoordExtensionsTests
    {
        static IEnumerable<object[]> CoordsTestDistanceData()
        {
            yield return new object[] {new Coord(0, 0), new Coord(0, 3), 3 };
            yield return new object[] {new Coord(1, 0), new Coord(5, 0), 4 };
            yield return new object[] {new Coord(0, 0), new Coord(0, 0), 0 };
        }
        static IEnumerable<object[]> CoordsTestSegmentsData()
        {
            yield return new object[] {new Coord(0, 3), new Coord(1, 3), new Coord(5, 3), false };
            yield return new object[] {new Coord(3, 3), new Coord(1, 3), new Coord(5, 3), true };
        }
        
        [Test]
        [TestCaseSource(nameof(CoordsTestDistanceData))]
        public void GetDistanceTo_Success(Coord a, Coord b, double expected)
        {
            var distance = a.GetDistanceTo(b);

            Assert.That(distance, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(CoordsTestSegmentsData))]
        public void IsOnSegment_Success(Coord a, Coord b, Coord c, bool expected)
        {
            var onSegment = a.IsOnSegment(b,c);

            Assert.That(onSegment, Is.EqualTo(expected));
        }
    }
}