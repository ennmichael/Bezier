using System.Windows.Controls;
using System.Windows.Media;

// TODO Benchmark how much faster it is to draw lines via deCasteljau's

namespace Bezier
{
    static class Drawing
    {
        public static int WeightIndicatorDiameter = 6;

        public static void Clear(Canvas canvas) => canvas.Children.Clear();

        public static void DrawEverything(Canvas canvas, ICurve curve, int steps)
        {
            DrawBoundingBox(canvas, curve);
            DrawCurve(canvas, curve, steps, Brushes.Black);
            DrawWeights(canvas, curve);
            DrawExtrema(canvas, curve);
        }

        public static void DrawWeights(Canvas canvas, ICurve curve)
        {
            foreach (var weight in curve.Weights)
                DrawWeight(canvas, weight);
        }

        private static void DrawWeight(Canvas canvas, Vector2 weight) => DrawPointMarker(canvas, weight, Brushes.Aqua, 2);

        private static void DrawPointMarker(Canvas canvas, Vector2 point, Brush stroke, int strokeThickness)
        {
            var ellipse = new System.Windows.Shapes.Ellipse
            {
                Width = WeightIndicatorDiameter,
                Height = WeightIndicatorDiameter,
                Stroke = stroke,
                StrokeThickness = strokeThickness
            };
            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);
            canvas.Children.Add(ellipse);
        }

        public static void DrawCurve(Canvas canvas, ICurve curve, int steps, Brush stroke)
        {
            float delta = 1.0f / steps;
            Vector2 previousPoint = curve.Point(0.0f);
            for (float t = delta; t < 1.0f; t += delta)
            {
                var point = curve.Point(t);
                DrawLine(canvas, previousPoint, point, stroke);
                previousPoint = point;
            }
            DrawLine(canvas, previousPoint, curve.Point(1.0f), stroke);
        }

        private static void DrawLine(Canvas canvas, Vector2 from, Vector2 to, Brush stroke)
        {
            var line = new System.Windows.Shapes.Line
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                Stroke = stroke,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);
        }

        public static void DrawBoundingBox(Canvas canvas, ICurve curve) =>
            DrawRectangle(canvas, curve.BoundingBox(offset: 2.0f), Brushes.LightGreen, 3);

        private static void DrawRectangle(Canvas canvas, Rectangle rectangle, Brush stroke, int thickness) =>
            canvas.Children.Add(rectangle.ToWindowsRectangle(stroke, thickness));

        public static void DrawExtrema(Canvas canvas, ICurve curve)
        {
            foreach (Vector2 extreme in curve.Extrema())
                DrawPointMarker(canvas, extreme, Brushes.Red, 3);
        }
    }
}
