#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using ShapeGenerator;
using ShapesRecognizer;
using ShapesRecognizer.Shapes;
using SvgImageCreator;

namespace ConsoleApp_Shapes
{
    static class Program
    {
        static void Main()
        {
            Recognizer recognizer = new Recognizer();
            List<Shape> shapes = new List<Shape>();
            SvgBuilder svgBuilder = new SvgBuilder();
            Generator.SetWindowSize(600, 600);
            shapes.AddRange(Generator.GenerateAllShapes(100, 50, 30, false));

            //U can subscribe to event (for example to draw New Items one by one )
            recognizer.ObservableShapeOnTopIds.CollectionChanged += CollectionChanged;
            var shapesOnTopId = recognizer.FindAllTopByAreaThreshold(shapes);
            
            foreach (var shape in shapes)
            {
                if (shapesOnTopId.Contains(shape.Id))
                {
                    shape.Color = Color.Red;
                    if (shape is Line line) 
                    {
                        line.OutLine = Color.Red;
                    }
                }

                svgBuilder.Add(shape);
            }

            svgBuilder.RetrieveSvg();
        }
        
        private static void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var newEl = e.NewItems?[0];
            if (newEl is not null)
            {
                Console.WriteLine($"collection changed added {(int)newEl}");
            }
            var removeEl = e.OldItems?[0];
            if (removeEl is not null)
            {
                Console.WriteLine($"collection changed removed {(int)removeEl}");
            }
        }
    }
}