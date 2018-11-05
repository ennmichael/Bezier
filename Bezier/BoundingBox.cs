using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    static class BoundingBoxExtension
    {
        public static IEnumerable<Vector2> Extrema(this ICurve curve) => FindDerivativeRoots(curve).Select(t => curve.Point(t));

        public static Rectangle BoundingBox(this ICurve curve, float offset = 0)
        {
            var criticalPoints = FindCriticalPoints(curve).ToArray();
            var xs = criticalPoints.Select(p => p.X);
            var ys = criticalPoints.Select(p => p.Y);
            float minX = xs.Min();
            float maxX = xs.Max();
            float minY = ys.Min();
            float maxY = ys.Max();
            return new Rectangle(
                lowerLeft: new Vector2(minX - offset, maxY + offset),
                upperRight: new Vector2(maxX + offset, minY - offset));
        }

        private static IEnumerable<Vector2> FindCriticalPoints(ICurve curve)
        {
            var p1 = FindDerivativeRoots(curve).Select(root => curve.Point(root));
            var p2 = new[] { curve.Weights.First(), curve.Weights.Last() };
            return p1.Concat(p2);
        }

        private static IEnumerable<float> FindDerivativeRoots(ICurve curve)
        {
            ICurve firstDerivative = curve.Derivative;
            if (firstDerivative == null)
                yield break;
            foreach (float t in firstDerivative.Roots)
                yield return t;

            ICurve secondDerivative = firstDerivative.Derivative;
            if (secondDerivative == null)
                yield break;
            foreach (float t in secondDerivative.Roots)
                yield return t;
        }
    }
}
