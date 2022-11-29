using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ShapeGenerator;
using ShapesRecognizer;
using ShapesRecognizer.Common;
using ShapesRecognizer.Shapes;

namespace ShapesRecognizerTests
{
    public class RecognizerTests
    {
        [Test]
        [TestCaseSource(nameof(IntersectionTestData))]
        public void FindAllTop_GetAllTop_Success(List<Shape> shapes, int expectedCount, List<int> expectedIds)
        {
            var recognizer = new Recognizer();
            var topShapes = recognizer.FindAllTopByAreaThreshold(shapes);
            
            Assert.That(topShapes, Is.EquivalentTo(expectedIds));
            Assert.That(topShapes.Count, Is.EqualTo(expectedCount));
        }
        
        [Test]
        [TestCaseSource(nameof(IntersectionTestData))]
        public void FindAllTopByAreaThreshold_GetAllTopByArea_Success(List<Shape> shapes, int expectedCount, List<int> expectedIds)
        {
            var recognizer = new Recognizer();
            var area = 50;
            var topShapesId = recognizer.FindAllTopByAreaThreshold(shapes, (uint)area);
            var topShapes = shapes.Where(s => topShapesId.Contains(s.Id)).ToList();
            
            Assert.That(shapes.All(s=>s.GetArea() >= area));
            Assert.That(topShapes.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        [TestCaseSource(nameof(_counts))]
        public void FindTopByCount_Success(int countToGenerate, int expectedCount)
        {
            var recognizer = new Recognizer();
            var shapes = new List<Shape>();
            shapes.AddRange(Generator.GenerateAllShapes((uint)countToGenerate,30,20));
            var topShapesId = recognizer.FindAllTopByAreaThreshold(shapes, count: (uint)expectedCount);
            
            Assert.That(topShapesId.Count, Is.EqualTo(expectedCount));
        }

        private static object[] _counts =
        {
            new object[] { 200, 5 },
            new object[] { 200, new Random().Next(50) },
            new object[] { 200, 47 },

        };
        static IEnumerable<object[]> IntersectionTestData()
        {
            var shapes = new List<Shape>();
            var ids = new List<int>();
            #region 1TestCase_TwoCirclesAboveOthers

            for (int i = 0; i < 5; i++)
            {
                shapes.Add(new Circle(new Coord(i * 20 + 10, 50), 10));
            }

            var circle1 = new Circle(new Coord(30, 50), 20);
            shapes.Add(circle1);
            ids.Add(circle1.Id);

            var circle2 = new Circle(new Coord(70, 50), 20);
            shapes.Add(circle2);
            ids.Add(circle2.Id);
            
            yield return new object[] { shapes, 2, ids };

            #endregion

            #region 2TestCase_RectangleAboveCircles

            shapes = new List<Shape>();
            ids = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                shapes.Add(new Circle(new Coord(i * 20 + 10, 50), 10));
            }

            var topRect = new Rectangle(new Coord(0, 0), 100, 100);
            shapes.Add(topRect);
            ids.Add(topRect.Id);

            yield return new object[] { shapes, 1, ids };

            #endregion
            
            #region 3TestCase_RectangleAboveRectangle

            shapes = new List<Shape>();
            ids = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                shapes.Add(new Circle(new Coord(i * 20 + 10, 50), 10));
            }

            shapes.Add(new Rectangle(new Coord(0, 0), 100, 100));

            topRect = new Rectangle(new Coord(10, 10), 50, 50);
            shapes.Add(topRect);
            ids.Add(topRect.Id);

            yield return new object[] { shapes, 1, ids};

            #endregion
            
            #region 4TestCase_TwoTrianglesNear

            shapes = new List<Shape>();
            ids = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                shapes.Add(new Circle(new Coord(i * 20 + 10, 50), 10));
            }

            var tr1 = new Triangle(new Coord(0, 0), new Coord(99, 0), new Coord(0, 99));
            shapes.Add(tr1);
            ids.Add(tr1.Id);

            var tr2 = new Triangle(new Coord(100, 100), new Coord(100, 0), new Coord(0, 100));
            shapes.Add(tr2);
            ids.Add(tr2.Id);
            
            yield return new object[] { shapes, 2 , ids};

            #endregion
            
            #region 5TestCase_ParallelLinesAboveAll

            shapes = new List<Shape>();
            ids = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                shapes.Add(new Circle(new Coord(i * 20 + 10, 50), 10));
            }
            shapes.Add(new Triangle(new Coord(100, 100), new Coord(100, 0), new Coord(0, 100)));


            var line = new Line(new Coord(0, 50), new Coord(100, 50));
            shapes.Add(line);
            ids.Add(line.Id);

            var line2 = new Line(new Coord(0, 60), new Coord(100, 60));
            shapes.Add(line2);
            ids.Add(line2.Id);

            yield return new object[] { shapes, 2, ids };

            #endregion
            
            #region 6TestCase_IntersectedLinesAboveAll
            shapes = new List<Shape>();
            ids = new List<int>();
            shapes.Add(new Line(new Coord(0, 50), new Coord(100, 50)));

            var topLine = new Line(new Coord(100, 60), new Coord(0, 0));
            shapes.Add(topLine);
            ids.Add(topLine.Id);

            yield return new object[] { shapes, 1, ids };

            #endregion
            
            #region 7TestCase_TriangleAboveAll
            shapes = new List<Shape>();
            ids = new List<int>();
            
            shapes.Add(new Line(new Coord(0, 50), new Coord(100, 50)));
            shapes.Add( new Line(new Coord(100, 60), new Coord(0, 0)));
            shapes.Add(new Rectangle(new Coord(0, 0), 50, 100));
            shapes.Add(new Line(new Coord(0, 50), new Coord(100, 50)));
            shapes.Add(new Circle(new Coord(0, 50), 60));
            shapes.Add(new Triangle(new Coord(0, 0), new Coord(10, 0), new Coord(0, 99)));

            tr1 = new Triangle(new Coord(0, 0), new Coord(99, 0), new Coord(0, 99));
            shapes.Add(tr1);
            ids.Add(tr1.Id);
            
            yield return new object[] { shapes, 1, ids };

            #endregion
        }
    }
}