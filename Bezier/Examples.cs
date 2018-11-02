using System.Windows.Controls;

namespace Bezier
{
    static class Examples
    {
        public static void RoughDrawExample(Canvas canvas)
        {
            var bezierCurve = new CubicBezierCurve(
                new Vector2(10.0f, 10.0f),
                new Vector2(20.0f, 80.0f),
                new Vector2(20.0f, 200.0f),
                new Vector2(100.0f, 100.0f));
            BezierDrawing.DrawWeights(canvas, bezierCurve);
            BezierDrawing.Draw(canvas, bezierCurve, 0.02f);
        }
    }
}
