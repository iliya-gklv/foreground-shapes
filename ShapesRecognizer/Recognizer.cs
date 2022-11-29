using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShapesRecognizer.Common;
using ShapesRecognizer.Shapes;

namespace ShapesRecognizer
{
    /// <summary>
    /// Used to determine foreground shapes in lis by physical order (id)
    /// </summary>
    public class Recognizer
    {
        public readonly ObservableCollection<int> ObservableShapeOnTopIds = new();

        /// <summary>
        /// Finds all top shapes ID which satisfy the given condition by area and count
        /// </summary>
        /// <param name="shapes">Input shapes</param>
        /// <param name="minArea">Min threshold, algorithm returns all shapes with area greater or equal than this value</param>
        /// <param name="count">Returns exact amount of top shapes</param>
        /// <param name="knownShapesOnTopId">Can be used if some input data is already known</param>
        /// <returns>List of foreground shapes ID</returns>
        public List<int> FindAllTopByAreaThreshold(List<Shape> shapes, uint minArea = 0, uint count = 0,
            List<int> knownShapesOnTopId = null)
        {
            var result = FindOnTop(shapes, count, knownShapesOnTopId).ToList();
            if (minArea > 0)
            {
                var idToRemoveList = shapes.Where(shape => result.Contains(shape.Id) && shape.GetArea() < minArea)
                    .Select(s => s.Id).ToList();
                result.RemoveAll(id => idToRemoveList.Contains(id));
                return result;
            }

            return result;
        }

        /// <summary>
        /// Finds all top shapes ID which satisfy the given optional condition by count and known shapes id list
        /// </summary>
        /// <param name="shapes">Input shapes</param>
        /// <param name="count">Returns exact amount of top shapes</param>
        /// <param name="knownShapesOnTopId">Can be used if some input data is already known</param>
        /// <returns>IEnumerable of foreground shapes ID</returns>
        private IEnumerable<int> FindOnTop(List<Shape> shapes, uint count = 0, List<int> knownShapesOnTopId = null)
        {
            IEnumerable<int> result;

            if (knownShapesOnTopId is null)
            {
                knownShapesOnTopId = new List<int>();
                result = FindAllOnTop(shapes, knownShapesOnTopId);
            }
            else
            {
                result = FindAllOnTop(shapes, knownShapesOnTopId);
            }

            if (count > 0)
            {
                result = result.TakeLast((int)count);
            }

            return result;
        }

        /// <summary>
        /// Finds all top shapes ID which satisfy the given  by known shapes
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="knownShapesOnTopId"></param>
        /// <returns></returns>
        private IEnumerable<int> FindAllOnTop(List<Shape> shapes, List<int> knownShapesOnTopId)
        {
            var hasIntersection = false;
            var shapeOnTopIds = new List<int>();

            var shapesAlreadyCheckedIds = new List<int>();
            shapeOnTopIds.AddRange(shapes.Select(shape => shape.Id).ToList());


            if (knownShapesOnTopId.Count > 0)
            {
                shapesAlreadyCheckedIds.AddRange(knownShapesOnTopId);
            }

            foreach (var shape in shapes)
            {
                if (shapesAlreadyCheckedIds.Contains(shape.Id))
                {
                    continue;
                }

                var lonelyShape = true;
                foreach (var anotherShape in shapes)
                {
                    if (shape.Id == anotherShape.Id) continue;

                    if (shapesAlreadyCheckedIds.Contains(anotherShape.Id))
                    {
                        continue;
                    }

                    switch (shape)
                    {
                        case Rectangle rect:
                            switch (anotherShape)
                            {
                                case Rectangle anotherRect:
                                    hasIntersection = HasIntersection(rect, anotherRect);
                                    break;
                                case Circle anotherCircle:
                                {
                                    hasIntersection = HasIntersection(rect, anotherCircle);
                                    break;
                                }
                                case Triangle anotherTriangle:
                                    hasIntersection = HasIntersection(rect, anotherTriangle);
                                    break;
                                case Line anotherLine:
                                    hasIntersection = HasIntersection(rect, anotherLine);
                                    break;
                            }

                            break;
                        case Circle circle:
                            switch (anotherShape)
                            {
                                case Circle anotherCircle:
                                    hasIntersection = HasIntersection(circle, anotherCircle);
                                    break;
                                case Rectangle rectangle:
                                    hasIntersection = HasIntersection(rectangle, circle);
                                    break;
                                case Triangle anotherTriangle:
                                    hasIntersection = HasIntersection(circle, anotherTriangle);
                                    break;
                                case Line anotherLine:
                                    hasIntersection = HasIntersection(circle, anotherLine);
                                    break;
                            }

                            break;
                        case Triangle triangle:
                        {
                            switch (anotherShape)
                            {
                                case Triangle anotherTriangle:
                                    hasIntersection = HasIntersection(triangle, anotherTriangle);
                                    break;
                                case Circle anotherCircle:
                                    hasIntersection = HasIntersection(anotherCircle, triangle);
                                    break;
                                case Rectangle rectangle:
                                    hasIntersection = HasIntersection(rectangle, triangle);
                                    break;
                                case Line anotherLine:
                                    hasIntersection = HasIntersection(triangle, anotherLine);
                                    break;
                            }

                            break;
                        }
                        case Line line:
                        {
                            switch (anotherShape)
                            {
                                case Line anotherLine:
                                    hasIntersection = HasIntersection(line, anotherLine);
                                    break;
                                case Triangle anotherTriangle:
                                    hasIntersection = HasIntersection(anotherTriangle, line);
                                    break;
                                case Circle anotherCircle:
                                    hasIntersection = HasIntersection(anotherCircle, line);
                                    break;
                                case Rectangle anotherRectangle:
                                    hasIntersection = HasIntersection(anotherRectangle, line);
                                    break;
                            }

                            break;
                        }
                    }

                    if (hasIntersection)
                    {
                        var topId = Math.Max(shape.Id, anotherShape.Id);
                        var idToRemove = shape.Id == topId ? anotherShape.Id : shape.Id;
                        ObservableShapeOnTopIds.Add(topId);
                        ObservableShapeOnTopIds.RemoveAll(id => id == idToRemove);
                        shapeOnTopIds.RemoveAll(id => id == idToRemove);
                        shapesAlreadyCheckedIds.Add(idToRemove);
                        hasIntersection = false;
                        lonelyShape = false;
                    }
                }

                if (lonelyShape && !ObservableShapeOnTopIds.Contains(shape.Id))
                {
                    ObservableShapeOnTopIds.Add(shape.Id);
                }
            }

            return shapeOnTopIds;
        }

        /// <summary>
        /// Used to determine the intersection between circle and triangle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="triangle"></param>
        /// <returns>true or false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private bool HasIntersection(Circle circle, Triangle triangle)
        {
            if (circle is null)
            {
                throw new ArgumentNullException(nameof(circle));
            }

            if (triangle is null)
            {
                throw new ArgumentNullException(nameof(triangle));
            }

            var triangleSides = GetTriangleSides(triangle);
            return triangleSides.Any(side => HasIntersection(circle, side)) ||
                   PointIsInsideTriangle(circle.Center, triangle);
        }

        /// <summary>
        /// Used to determine the intersection between circle and line
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <returns>true or false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private bool HasIntersection(Circle circle, Line line)
        {
            if (circle is null)
            {
                throw new ArgumentNullException(nameof(circle));
            }

            if (line is null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            var signedDistance = GetDistancePointToSegment(circle.Center, line);

            return signedDistance < circle.Radius;
        }


        private double GetDistancePointToSegment(Coord point, Line segment)
        {
            Coord vectorV = new Coord(segment.B.X - segment.A.X, segment.B.Y - segment.A.Y);
            Coord vectorW = new Coord(point.X - segment.A.X, point.Y - segment.A.Y);

            double c1 = Dot(vectorW, vectorV);
            if (c1 < 0)
            {
                return D(point, segment.A);
            }

            double c2 = Dot(vectorV, vectorV);
            if (c2 < c1)
            {
                return D(point, segment.B);
            }

            double b = c1 / c2;
            Coord pb = new Coord((int)(segment.A.X + b * vectorV.X), (int)(segment.A.Y + b * vectorV.Y));
            return D(point, pb);
        }

        private double D(Coord u, Coord v) => Norm(new Coord(u.X - v.X, u.Y - v.Y));
        private double Norm(Coord vector) => Math.Sqrt(Dot(vector, vector));

        private double Dot(Coord vector1, Coord vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        private bool HasIntersection(Triangle triangle, Triangle anotherTriangle)
        {
            if (triangle is null)
            {
                throw new ArgumentNullException(nameof(triangle));
            }

            if (anotherTriangle is null)
            {
                throw new ArgumentNullException(nameof(anotherTriangle));
            }

            var anotherTriangleSides = GetTriangleSides(anotherTriangle);
            var triangleSides = GetTriangleSides(triangle);

            return triangleSides.Any(side => HasIntersection(anotherTriangle, side)) ||
                   anotherTriangleSides.Any(side => HasIntersection(triangle, side));
        }

        private bool HasIntersection(Triangle triangle, Line line)
        {
            if (triangle is null)
            {
                throw new ArgumentNullException(nameof(triangle));
            }

            if (line is null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            if (PointIsInsideTriangle(line.A, triangle) || PointIsInsideTriangle(line.B, triangle))
            {
                return true;
            }

            var sides = GetTriangleSides(triangle);
            return sides.Any(side => HasIntersection(side, line));
        }

        private bool HasIntersection(Line line1, Line line2)
        {
            if (line1 is null)
            {
                throw new ArgumentNullException(nameof(line1));
            }

            if (line2 is null)
            {
                throw new ArgumentNullException(nameof(line2));
            }

            return DoIntersect(line1.A, line1.B, line2.A, line2.B);
        }

        private bool PointIsInsideTriangle(Coord point, Triangle triangle)
        {
            var inside = false;
            var abVector = GetVectorCoord(triangle.A, triangle.B);
            var apVector = GetVectorCoord(triangle.A, point);

            var bcVector = GetVectorCoord(triangle.B, triangle.C);
            var bpVector = GetVectorCoord(triangle.B, point);

            var caVector = GetVectorCoord(triangle.C, triangle.A);
            var cpVector = GetVectorCoord(triangle.C, point);

            var abXap = ZValueFromVectorProduct(abVector, apVector);
            var bcXbp = ZValueFromVectorProduct(bcVector, bpVector);
            var caXcp = ZValueFromVectorProduct(caVector, cpVector);

            if ((abXap > 0 && bcXbp > 0 && caXcp > 0) || (abXap < 0 && bcXbp < 0 && caXcp < 0))
            {
                inside = true;
            }

            //1 zero, others are all positive or all negative -> point on a side
            //2 zero -> point is on a point of a triangle

            // if positive and negative at the same time then the point is outside
            return inside;
        }

        private Coord GetVectorCoord(Coord aStart, Coord bEnd) => new(bEnd.X - aStart.X, bEnd.Y - aStart.Y);
        
        private int ZValueFromVectorProduct(Coord vectorA, Coord vectorB)
        {
            if (vectorA is null)
            {
                throw new ArgumentNullException(nameof(vectorA));
            }

            if (vectorB is null)
            {
                throw new ArgumentNullException(nameof(vectorB));
            }

            return vectorA.X * vectorB.Y - vectorA.Y * vectorB.X;
        }

        /// <summary>
        /// To find orientation of ordered triplet (p, q, r).
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns>
        /// The function returns following values: 
        /// 0 if p, q and r are collinear,
        /// 1 if Clockwise,
        /// 2 if Counterclockwise
        /// </returns>
        private Orientation GetOrientation(Coord p, Coord q, Coord r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return Orientation.Collinear;

            return val > 0 ? Orientation.Clockwise : Orientation.Counterclockwise;
        }

        /// <summary>
        /// Returns true if line segment 'p1q1' and 'p2q2' intersect.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="q1"></param>
        /// <param name="p2"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        bool DoIntersect(Coord p1, Coord q1, Coord p2, Coord q2)
        {
            // Find the four orientations needed for general and
            // special cases
            Orientation o1 = GetOrientation(p1, q1, p2);
            Orientation o2 = GetOrientation(p1, q1, q2);
            Orientation o3 = GetOrientation(p2, q2, p1);
            Orientation o4 = GetOrientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            // if (o1 == 0 && IsOnSegment(p1, p2, q1)) return true;
            if (o1 == 0 && p2.IsOnSegment(p1, q1)) return true;

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && q2.IsOnSegment(p1, q1)) return true;

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && p1.IsOnSegment(p2, q2)) return true;

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && q1.IsOnSegment(p2, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }

        private bool HasIntersection(Rectangle rect1, Rectangle rect2)
        {
            if (rect1 is null)
            {
                throw new ArgumentNullException(nameof(rect1));
            }

            if (rect2 is null)
            {
                throw new ArgumentNullException(nameof(rect2));
            }

            var x1 = rect1.TopLeft.X;
            var y1 = rect1.TopLeft.Y;
            var w1 = rect1.Width;
            var l1 = rect1.Length;

            var x2 = rect2.TopLeft.X;
            var y2 = rect2.TopLeft.Y;
            var w2 = rect2.Width;
            var l2 = rect2.Length;

            return x1 + w1 >= x2 && x1 <= x2 + w2 && y1 + l1 >= y2 && y1 <= y2 + l2;
        }

        private bool HasIntersection(Rectangle rectangle, Circle circle)
        {
            if (rectangle is null)
            {
                throw new ArgumentNullException(nameof(rectangle));
            }

            if (circle is null)
            {
                throw new ArgumentNullException(nameof(circle));
            }

            var rectSides = GetRectangleSides(rectangle);

            return rectSides.Any(side => HasIntersection(circle, side)) ||
                   PointIsInsideRectangle(circle.Center, rectangle);
        }

        private bool PointIsInsideRectangle(Coord point, Rectangle rectangle)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            if (rectangle is null)
            {
                throw new ArgumentNullException(nameof(rectangle));
            }

            var x = point.X;
            var y = point.Y;
            var topLeft = new Coord(rectangle.TopLeft);
            var topRight = new Coord(topLeft.X + (int)rectangle.Width, topLeft.Y);
            var bottomLeft = new Coord(topLeft.X, topLeft.Y + (int)rectangle.Length);

            return x > topLeft.X && x < topRight.X &&
                   y > topLeft.Y && y < bottomLeft.Y;
        }

        private bool HasIntersection(Circle circle1, Circle circle2)
        {
            var distanceBetweenCenters = circle1.Center.GetDistanceTo(circle2.Center);
            return distanceBetweenCenters < circle1.Radius + circle2.Radius;
        }

        private bool HasIntersection(Rectangle rectangle, Triangle triangle)
        {
            var rectSides = GetRectangleSides(rectangle);
            //intersection
            var triangleSides = GetTriangleSides(triangle);
            var hasIntersection = false;
            foreach (var rectSide in rectSides)
            {
                foreach (var triangleSide in triangleSides)
                {
                    hasIntersection = HasIntersection(rectSide, triangleSide);
                    if (hasIntersection) break;
                }

                if (hasIntersection) break;
            }

            var topLeft = new Coord(rectangle.TopLeft);
            var topRight = new Coord(topLeft.X + (int)rectangle.Width, topLeft.Y);
            var bottomLeft = new Coord(topLeft.X, topLeft.Y + (int)rectangle.Length);
            //inside rect
            if (IsBetween(triangle.A.X, topLeft.X, topRight.X) &&
                IsBetween(triangle.A.Y, topLeft.Y, bottomLeft.Y) &&
                IsBetween(triangle.B.X, topLeft.X, topRight.X) &&
                IsBetween(triangle.B.Y, topLeft.Y, bottomLeft.Y) &&
                IsBetween(triangle.C.X, topLeft.X, topRight.X) &&
                IsBetween(triangle.C.Y, topLeft.Y, bottomLeft.Y))
            {
                hasIntersection = true;
            }

            return hasIntersection;
        }

        private bool HasIntersection(Rectangle rectangle, Line line)
        {
            var hasIntersection = false;
            var rectSides = GetRectangleSides(rectangle);
            foreach (var rectSide in rectSides)
            {
                hasIntersection = HasIntersection(rectSide, line);
                if (hasIntersection)
                {
                    break;
                }
            }

            var topLeft = new Coord(rectangle.TopLeft);
            var topRight = new Coord(topLeft.X + (int)rectangle.Width, topLeft.Y);
            var bottomLeft = new Coord(topLeft.X, topLeft.Y + (int)rectangle.Length);
            //inside rect
            if (IsBetween(line.A.X, topLeft.X, topRight.X) &&
                IsBetween(line.A.Y, topLeft.Y, bottomLeft.Y) &&
                IsBetween(line.B.X, topLeft.X, topRight.X) &&
                IsBetween(line.B.Y, topLeft.Y, bottomLeft.Y))
            {
                hasIntersection = true;
            }

            return hasIntersection;
        }

        private bool IsBetween(int value, int min, int max)
        {
            return value < max && value > min;
        }

        private List<Line> GetRectangleSides(Rectangle rectangle)
        {
            var topLeft = new Coord(rectangle.TopLeft);
            var topRight = new Coord(topLeft.X + (int)rectangle.Width, topLeft.Y);
            var bottomLeft = new Coord(topLeft.X, topLeft.Y + (int)rectangle.Length);
            var bottomRight = new Coord(topRight.X, bottomLeft.Y);
            var leftSide = new Line(topLeft, bottomLeft);
            var rightSide = new Line(topRight, bottomRight);
            var topSide = new Line(topLeft, topRight);
            var bottomSide = new Line(bottomLeft, bottomRight);

            return new List<Line> { leftSide, rightSide, bottomSide, topSide };
        }

        private List<Line> GetTriangleSides(Triangle triangle)
        {
            var sideA = new Line(triangle.B, triangle.C);
            var sideB = new Line(triangle.A, triangle.C);
            var sideC = new Line(triangle.B, triangle.A);
            return new List<Line>() { sideA, sideB, sideC };
        }
    }
}