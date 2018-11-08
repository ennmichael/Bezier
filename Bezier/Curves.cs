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

        (ICurve, ICurve) Split(float z);
    }

    class QuarticCurve : ICurve
    {
        public IList<Vector2> Weights { get; }

        public QuarticCurve(Vector2 w0, Vector2 w1, Vector2 w2, Vector2 w3, Vector2 w4) =>
            Weights = new List<Vector2> { w0, w1, w2, w3, w4 };

        public Vector2 Point(float t) => DeCasteljau(t);

        public Vector2 DeCasteljau(float t)
        {
            Vector2 p0 = Vector2.Interpolate(Weights[0], Weights[1], t);
            Vector2 p1 = Vector2.Interpolate(Weights[1], Weights[2], t);
            Vector2 p2 = Vector2.Interpolate(Weights[2], Weights[3], t);
            Vector2 p3 = Vector2.Interpolate(Weights[3], Weights[4], t);
            Vector2 q0 = Vector2.Interpolate(p0, p1, t);
            Vector2 q1 = Vector2.Interpolate(p1, p2, t);
            Vector2 q2 = Vector2.Interpolate(p2, p3, t);
            Vector2 r0 = Vector2.Interpolate(q0, q1, t);
            Vector2 r1 = Vector2.Interpolate(q1, q2, t);
            return Vector2.Interpolate(r0, r1, t);
        }

        public Vector2 Bernstein(float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            float t4 = t3 * t;
            float u = 1.0f - t;
            float u2 = u * u;
            float u3 = u2 * u;
            float u4 = u3 * u;
            return u4 * Weights[0] + 4 * u3 * t * Weights[1] + 6 * u2 * t2 * Weights[2] + 4 * u * t3 * Weights[3] + t4 * Weights[4];
        }

        public (ICurve, ICurve) Split(float z)
        {
            throw new NotImplementedException();
        }

        ICurve ICurve.Derivative => Derivative;

        public CubicCurve Derivative =>
            new CubicCurve(
                4 * (Weights[1] - Weights[0]), 4 * (Weights[2] - Weights[1]),
                4 * (Weights[3] - Weights[2]), 4 * (Weights[4] - Weights[3]));

        public IEnumerable<float> Roots => throw new NotImplementedException();
    }

    class CubicCurve : ICurve
    {
        public IList<Vector2> Weights { get; }

        public CubicCurve(Vector2 w0, Vector2 w1, Vector2 w2, Vector2 w3) =>
            Weights = new List<Vector2> { w0, w1, w2, w3 };

        public Vector2 Point(float t) => DeCasteljau(t);

        public Vector2 DeCasteljau(float t)
        {
            Vector2 p0 = Vector2.Interpolate(Weights[0], Weights[1], t);
            Vector2 p1 = Vector2.Interpolate(Weights[1], Weights[2], t);
            Vector2 p2 = Vector2.Interpolate(Weights[2], Weights[3], t);
            Vector2 q0 = Vector2.Interpolate(p0, p1, t);
            Vector2 q1 = Vector2.Interpolate(p1, p2, t);
            return Vector2.Interpolate(q0, q1, t);
        }

        public Vector2 Bernstein(float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            float u = 1.0f - t;
            float u2 = u * u;
            float u3 = u2 * u;
            return u3 * Weights[0] + 3 * u2 * t * Weights[1] + 3 * u * t2 * Weights[2] + t3 * Weights[3];
        }

        public (ICurve, ICurve) Split(float z)
        {
            throw new NotImplementedException();
        }

        ICurve ICurve.Derivative => Derivative;

        public QuadraticCurve Derivative =>
            new QuadraticCurve(3 * (Weights[1] - Weights[0]), 3 * (Weights[2] - Weights[1]), 3 * (Weights[3] - Weights[2]));

        public IEnumerable<float> Roots
        {
            get
            {
                Vector2 w1Tripled = 3 * Weights[1];
                Vector2 w2Tripled = 3 * Weights[2];
                Vector2 wa = -Weights[0] + w1Tripled - w2Tripled + Weights[3];
                Vector2 wb = 3 * Weights[0] - 6 * Weights[1] + w2Tripled;
                Vector2 wc = -3 * Weights[0] + w1Tripled;
                Vector2 wd = Weights[0];
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

        public Vector2 Point(float t) => Bernstein(t);

        private Vector2 Bernstein(float t)
        {
            float u = 1.0f - t;
            return u * u * Weights[0] + 2 * u * t * Weights[1] + t * t * Weights[2];
        }

        (ICurve, ICurve) ICurve.Split(float z) => Split(z);

        public (QuadraticCurve, QuadraticCurve) Split(float z)
        {
            float u = 1 - z;
            float u2 = u * u;
            float z2 = z * z;
            float uzDoubled = 2 * u * z;
            var a = new QuadraticCurve(
                Weights[0],
                u * Weights[0] + z * Weights[1],
                u2 * Weights[0] + uzDoubled * Weights[1] + z2 * Weights[2]);
            var b = new QuadraticCurve(
                u2 * Weights[0] + uzDoubled * Weights[1] + z2 * Weights[2],
                u * Weights[1] + z * Weights[2],
                Weights[2]);
            return (a, b);
        }

        ICurve ICurve.Derivative => Derivative;

        public LinearCurve Derivative => new LinearCurve(2 * (Weights[1] - Weights[0]), 2 * (Weights[2] - Weights[1]));

        public IEnumerable<float> Roots
        {
            get
            {
                Vector2 w1Doubled = 2 * Weights[1];
                Vector2 wa = Weights[0] - w1Doubled + Weights[2];
                Vector2 wb = -(2 * Weights[0] - w1Doubled);
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

        public Vector2 Point(float t) => Bernstein(t);

        private Vector2 Bernstein(float t) => (1.0f - t) * Weights[0] + t * Weights[1];

        public (ICurve, ICurve) Split(float z)
        {
            throw new NotImplementedException();
        }

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
