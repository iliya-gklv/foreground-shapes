using System;
using System.IO;
using System.Reflection;
using System.Text;
using ShapesRecognizer.Shapes;
using Rectangle = ShapesRecognizer.Shapes.Rectangle;

namespace SvgImageCreator
{
    public class SvgBuilder
    {
        private readonly StringBuilder _stringBuilder = new();
        public uint MaxWidth { get; }
        public uint MaxLength { get; }

        public SvgBuilder(uint maxWidth = 800, uint maxLength = 600)
        {
            if (maxWidth == 0 || maxLength == 0)
            {
                throw new ArgumentException("Arguments can not be 0");
            }

            (MaxWidth, MaxLength) = (maxWidth, maxLength);
            _stringBuilder.AppendLine(
                $"<svg width=\"{maxWidth}\" height=\"{maxLength}\" xmlns=\"http://www.w3.org/2000/svg\">");
            _stringBuilder.AppendLine("<g>");
        }

        //toDo: add path where to save
        public bool RetrieveSvg()
        {
            _stringBuilder.AppendLine("</g></svg>");
            var svgContent = _stringBuilder.ToString();
            _stringBuilder.Clear();

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var name = $"svg_{dateTime}.svg";
            if (directory is null)
            {
                return false;
            }

            var path = Path.Combine(directory, name);
            File.WriteAllText(path, svgContent);
            return File.Exists(path);
        }

        public SvgBuilder Add(Shape shape, bool isFilled = true)
        {
            switch (shape)
            {
                case Rectangle rectangle:
                {
                    AddRect(rectangle);
                    break;
                }
                case Circle circle:
                {
                    AddCircle(circle);
                    break;
                }
                case Triangle triangle:
                {
                    AddTriangle(triangle);
                    break;
                }
                case Line line:
                {
                    AddLine(line);
                    break;
                }
            }

            return this;
        }
        private SvgBuilder AddRect(Rectangle rectangle, bool isFilled = true)
        {
            var fillHex = isFilled ? rectangle.Color.ToHexFormat() : "none";
            _stringBuilder.AppendLine(
                $"<rect id=\"svg_{rectangle.Id}\" " +
                $"height=\"{rectangle.Length}\" " +
                $"width=\"{rectangle.Width}\" " +
                $"y=\"{rectangle.TopLeft.Y}\" " +
                $"x=\"{rectangle.TopLeft.X}\" " +
                $"stroke=\"{rectangle.OutLine.ToHexFormat()}\" " +
                $"fill=\"{fillHex}\"/>");
            return this;
        }

        private SvgBuilder AddTriangle(Triangle triangle, bool isFilled = true)
        {
            var fillHex = isFilled ? triangle.Color.ToHexFormat() : "none";
            _stringBuilder.AppendLine("<polygon " +
                                      $"id=\"svg_{triangle.Id}\" " +
                                      "points=\"" +
                                      $"{triangle.A.X} {triangle.A.Y}, " +
                                      $"{triangle.B.X} {triangle.B.Y}, " +
                                      $"{triangle.C.X} {triangle.C.Y}\" " +
                                      $"stroke=\"{triangle.OutLine.ToHexFormat()}\" " +
                                      $"fill=\"{fillHex}\"/>");

            return this;
        }

        private SvgBuilder AddCircle(Circle circle, bool isFilled = true)
        {
            var fillHex = isFilled ? circle.Color.ToHexFormat() : "none";
            _stringBuilder.AppendLine("<circle " +
                                      $"id=\"svg_{circle.Id}\" " +
                                      $"cx=\"{circle.Center.X}\" " +
                                      $"cy=\"{circle.Center.Y}\" " +
                                      $"r=\"{circle.Radius}\" " +
                                      $"stroke=\"{circle.OutLine.ToHexFormat()}\" " +
                                      $"fill=\"{fillHex}\"/>");
            return this;
        }

        private SvgBuilder AddLine(Line line)
        {
            _stringBuilder.AppendLine("<line stroke-linecap=\"undefined\" " +
                                      "stroke-linejoin=\"undefined\" " +
                                      $"id=\"svg_{line.Id}\" " +
                                      $"y2=\"{line.B.Y}\" " +
                                      $"x2=\"{line.B.X}\" " +
                                      $"y1=\"{line.A.Y}\" " +
                                      $"x1=\"{line.A.X}\" " +
                                      $"stroke=\"{line.OutLine.ToHexFormat()}\" />");
            return this;
        }
    }
}