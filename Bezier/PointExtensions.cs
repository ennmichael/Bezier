using System.Windows;

namespace Bezier
{
    static class PointExtensions
    {
        public static Vector2 ToVector2(this Point point) => new Vector2((float)point.X, (float)point.Y);
    }
}
