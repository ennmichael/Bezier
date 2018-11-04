﻿using System.Windows.Controls;
using System.Windows.Media;

// TODO Benchmark how much faster it is to draw lines via deCasteljau's

namespace Bezier
{
    static class BezierDrawing
    {
        public static int WeightIndicatorDiameter = 6;

        public static void Clear(Canvas canvas) => canvas.Children.Clear();

        public static void DrawEverything(Canvas canvas, IBezierCurve bezierCurve, int steps)
        {
            DrawWeights(canvas, bezierCurve);
            DrawCurve(canvas, bezierCurve, steps);
        }

        public static void DrawWeights(Canvas canvas, IBezierCurve bezierCurve)
        {
            foreach (var weight in bezierCurve.Weights)
                DrawWeight(canvas, weight);
        }

        private static void DrawWeight(Canvas canvas, Vector2 weight)
        {
            var ellipse = new System.Windows.Shapes.Ellipse
            {
                Width = WeightIndicatorDiameter,
                Height = WeightIndicatorDiameter,
                Stroke = Brushes.Aqua,
                StrokeThickness = 2
            };
            Canvas.SetLeft(ellipse, weight.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, weight.Y - ellipse.Height / 2);
            canvas.Children.Add(ellipse);
        }

        public static void DrawCurve(Canvas canvas, IBezierCurve bezierCurve, int steps)
        {
            float delta = 1.0f / steps;
            Vector2 previousPoint = bezierCurve.Point(0.0f);
            for (float t = delta; t < 1.0f; t += delta)
            {
                var point = bezierCurve.Point(t);
                DrawLine(canvas, previousPoint, point);
                previousPoint = point;
            }
            DrawLine(canvas, previousPoint, bezierCurve.Point(1.0f));
        }

        private static void DrawLine(Canvas canvas, Vector2 from, Vector2 to)
        {
            var line = new System.Windows.Shapes.Line
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);
        }
    }
}
