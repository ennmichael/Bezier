using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

// TODO Benchmark how much faster it is to draw lines via deCasteljau's

namespace Bezier
{
    static class Drawing
    {
        private static readonly Brush skeletonStroke = Brushes.LightGray;

        private static readonly int skeletonThickness = 1;

        private static readonly Brush weightsStroke = Brushes.Black;

        private static readonly int weightsThickness = 2;

        private static readonly Brush curveStroke = Brushes.Black;

        private static readonly int curveThickness = 2;

        private static readonly Brush boundingBoxStroke = Brushes.LightGreen;

        private static readonly int boundingBoxThickness = 2;

        private static readonly float boundingBoxOffset = 2.0f;

        private static readonly Brush extremaStroke = Brushes.Red;

        private static readonly int extremaThickness = 2;

        private static readonly Brush intersectionStroke = Brushes.Blue;

        private static readonly int intersectionThickness = 2;

        public static int WeightIndicatorDiameter = 6;

        public static void Clear(this Canvas canvas) => canvas.Children.Clear();

        public static void DrawIntersections(this Canvas canvas, ICurve a, ICurve b)
        {
            foreach (Vector2 intersection in a.Intersections(b))
                canvas.DrawPointMarker(intersection, intersectionStroke, intersectionThickness);
        }

        public static void DrawSplit(this Canvas canvas, ICurve curve, float z, int steps)
        {
            (ICurve a, ICurve b) = curve.Split(z);
            canvas.DrawCurve(a, steps, Brushes.LightPink);
            canvas.DrawCurve(b, steps, Brushes.Blue);
        }

        public static void DrawSkeleton(this Canvas canvas, ICurve curve)
        {
            Vector2 previousPoint = curve.Weights.First();
            foreach (Vector2 currentPoint in curve.Weights.Skip(1))
            {
                canvas.DrawLine(previousPoint, currentPoint, skeletonStroke, skeletonThickness);
                previousPoint = currentPoint;
            }
        }

        public static void DrawWeights(this Canvas canvas, ICurve curve)
        {
            foreach (var weight in curve.Weights)
                canvas.DrawWeight(weight);
        }

        private static void DrawWeight(this Canvas canvas, Vector2 weight) =>
            canvas.DrawPointMarker(weight, weightsStroke, weightsThickness);

        private static void DrawPointMarker(this Canvas canvas, Vector2 point, Brush stroke, int strokeThickness)
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

        public static void DrawCurve(this Canvas canvas, ICurve curve, int precision, Brush stroke = null, int? strokeThickness = null)
        {
            stroke = stroke ?? curveStroke;
            int thickness = strokeThickness ?? curveThickness;
            float delta = 1.0f / precision;
            Vector2 previousPoint = curve.Point(0.0f);
            for (float t = delta; t < 1.0f; t += delta)
            {
                var point = curve.Point(t);
                canvas.DrawLine(previousPoint, point, stroke, thickness);
                previousPoint = point;
            }
            canvas.DrawLine(previousPoint, curve.Point(1.0f), stroke, thickness);
        }

        private static void DrawLine(this Canvas canvas, Vector2 from, Vector2 to, Brush stroke, int strokeThickness)
        {
            var line = new System.Windows.Shapes.Line
            {
                X1 = from.X,
                Y1 = from.Y,
                X2 = to.X,
                Y2 = to.Y,
                Stroke = stroke,
                StrokeThickness = strokeThickness
            };
            canvas.Children.Add(line);
        }

        public static void DrawBoundingBox(this Canvas canvas, ICurve curve)
        {
            Rectangle boundingBox = curve.BoundingBox(boundingBoxOffset);
            canvas.DrawRectangle(boundingBox, boundingBoxStroke, boundingBoxThickness);
        }

        private static void DrawRectangle(this Canvas canvas, Rectangle rectangle, Brush stroke, int thickness) =>
            canvas.Children.Add(rectangle.ToWindowsRectangle(stroke, thickness));

        public static void DrawExtrema(this Canvas canvas, ICurve curve)
        {
            foreach (Vector2 extreme in curve.Extrema())
                canvas.DrawPointMarker(extreme, extremaStroke, extremaThickness);
        }
    }
}
