using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bezier
{
    static class PointExtensions
    {
        public static Vector2 ToVector2(this Point point) => new Vector2((float)point.X, (float)point.Y);
    }

    static class Vector2Extensions
    {
        public static Point ToWindowsPoint(this Vector2 vector) => new Point(vector.X, vector.Y);
    }

    static class RectangleExtensions
    {
        public static System.Windows.Shapes.Rectangle ToWindowsRectangle(this Rectangle rectangle, Brush stroke, int thickness)
        {
            var result = new System.Windows.Shapes.Rectangle
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Stroke = stroke,
                StrokeThickness = thickness
            };
            var upperLeft = rectangle.UpperLeft;
            Canvas.SetLeft(result, upperLeft.X);
            Canvas.SetTop(result, upperLeft.Y);
            return result;
        }
    }
}
