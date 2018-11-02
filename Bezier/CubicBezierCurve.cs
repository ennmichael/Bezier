using System;
using System.Collections.Generic;

namespace Bezier
{
    class CubicBezierCurve : IBezierCurve
    {
        private static readonly int numWeights = 4;

        public IList<Vector2> Weights { get; }

        public CubicBezierCurve(Vector2 w0, Vector2 w1, Vector2 w2, Vector2 w3) =>
            Weights = new List<Vector2> { w0, w1, w2, w3 };

        public CubicBezierCurve(IList<Vector2> weights)
        {
            if (weights.Count != numWeights)
                throw new ArgumentException($"Cubic bezier curve must have exactly {numWeights} weights.");
            Weights = weights;
        }

        public Vector2 Point(float t) => DeCasteljau(t);

        public Vector2 DeCasteljau(float t)
        {
            if (!CheckT(t))
                throw new ArgumentOutOfRangeException();
            Vector2 p1 = Vector2.Interpolate(Weights[0], Weights[1], t);
            Vector2 p2 = Vector2.Interpolate(Weights[1], Weights[2], t);
            Vector2 p3 = Vector2.Interpolate(Weights[2], Weights[3], t);
            Vector2 q1 = Vector2.Interpolate(p1, p2, t);
            Vector2 q2 = Vector2.Interpolate(p2, p3, t);
            return Vector2.Interpolate(q1, q2, t);
        }

        public Vector2 Bernstein(float t)
        {
            if (!CheckT(t))
                throw new ArgumentOutOfRangeException();
            float u = (1 - t);
            float t2 = t * t;
            float u2 = u * u;
            float m0 = u2 * u;
            float m1 = 3 * u2 * t;
            float m2 = 3 * u * t2;
            float m3 = t2 * t;
            return m0 * Weights[0] + m1 * Weights[1] + m2 * Weights[2] + m3 * Weights[3];
        }

        private bool CheckT(float t) => t >= 0.0f && t <= 1.0f;
    }
}
