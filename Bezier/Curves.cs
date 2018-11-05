using System;
using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    interface ICurve
    {
        IList<Vector2> Weights { get; }

        Vector2 Point(float t);

        ICurve Derivative { get; }

        IEnumerable<float> Roots { get; }
    }

    class CubicCurve : ICurve
    {
        public IList<Vector2> Weights { get; }

        public CubicCurve(Vector2 w0, Vector2 w1, Vector2 w2, Vector2 w3) =>
            Weights = new List<Vector2> { w0, w1, w2, w3 };

        public Vector2 Point(float t) => DeCasteljau(t);

        public Vector2 DeCasteljau(float t)
        {
            if (!Utils.CheckT(t))
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
            if (!Utils.CheckT(t))
                throw new ArgumentOutOfRangeException();
            float u = 1 - t;
            float t2 = t * t;
            float u2 = u * u;
            float m0 = u2 * u;
            float m1 = 3 * u2 * t;
            float m2 = 3 * u * t2;
            float m3 = t2 * t;
            return m0 * Weights[0] + m1 * Weights[1] + m2 * Weights[2] + m3 * Weights[3];
        }

        ICurve ICurve.Derivative => Derivative;

        public QuadraticCurve Derivative =>
            new QuadraticCurve(3 * (Weights[1] - Weights[0]), 3 * (Weights[2] - Weights[1]), 3 * (Weights[3] - Weights[2]));

        public IEnumerable<float> Roots
        {
            get
            {
                Vector2 wa = Weights[0] + Weights[1] + Weights[2] + Weights[3];
                Vector2 wb = -(3 * Weights[0] + 2 * Weights[1] + Weights[2]);
                Vector2 wc = 3 * Weights[0] + Weights[1];
                Vector2 wd = -Weights[0];
                return Enumerable.Concat(
                    Equation.SolveCubicForT(wa.X, wb.X, wc.X, wd.X),
                    Equation.SolveCubicForT(wa.Y, wb.Y, wc.Y, wd.Y));
            }
        }
    }

    class QuadraticCurve : ICurve
    {
        public IList<Vector2> Weights { get; }

        public QuadraticCurve(Vector2 w0, Vector2 w1, Vector2 w2) =>
            Weights = new List<Vector2> { w0, w1, w2 };

        public Vector2 Point(float t)
        {
            float u = 1 - t;
            return u * u * Weights[0] + 2 * u * t * Weights[1] + t * t * Weights[2];
        }

        ICurve ICurve.Derivative => Derivative;

        public LinearCurve Derivative => new LinearCurve(2 * (Weights[1] - Weights[0]), 2 * (Weights[2] - Weights[1]));

        public IEnumerable<float> Roots
        {
            get
            {
                Vector2 w1Double = 2 * Weights[1];
                Vector2 wa = Weights[0] - w1Double + Weights[2];
                Vector2 wb = -(2 * Weights[0] - w1Double);
                Vector2 wc = Weights[0];
                return Enumerable.Concat(
                    Equation.SolveQuadraticForT(wa.X, wb.X, wc.X),
                    Equation.SolveQuadraticForT(wa.Y, wb.Y, wc.Y));
            }
        }
    }

    class LinearCurve : ICurve
    {
        public IList<Vector2> Weights { get; }

        public LinearCurve(Vector2 w0, Vector2 w1) =>
            Weights = new List<Vector2> { w0, w1 };

        public Vector2 Point(float t) => (1 - t) * Weights[0] + t * Weights[1];

        public ICurve Derivative => null;

        public IEnumerable<float> Roots
        {
            get
            {
                Vector2 wa = Weights[1] - Weights[0];
                Vector2 wb = Weights[0];
                float? t1 = Equation.SolveLinearForT(wa.X, wb.X);
                if (t1.HasValue)
                    yield return t1.Value;
                float? t2 = Equation.SolveLinearForT(wa.Y, wb.Y);
                if (t2.HasValue)
                    yield return t2.Value;
            }
        }
    }
}
