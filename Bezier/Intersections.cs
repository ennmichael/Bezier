using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    static class Intersections
    {
        public static IEnumerable<float> Find(LinearBezierCurve a, LinearBezierCurve b)
        {
            Vector2 numerator = a.Weights[0] - b.Weights[0];
            Vector2 denominator = numerator + a.Weights[1] - b.Weights[1];
            float t1 = numerator.X / denominator.X;
            float t2 = numerator.Y / denominator.Y;
            if (SolutionIsCorrect(t1, t2))
                yield return t1;
        }

        public static IEnumerable<float> Find(SquareBezierCurve a, LinearBezierCurve b) => Find(b, a);

        public static IEnumerable<float> Find(LinearBezierCurve a, SquareBezierCurve b)
        {
            Vector2 u = 2 * b.Weights[1];
            Vector2 wa = -(b.Weights[0] + u + b.Weights[2]);
            Vector2 wb = a.Weights[0] + a.Weights[1] + 2 * b.Weights[0] + u;
            Vector2 wc = -(a.Weights[0] + b.Weights[0]);
            return QuadraticSolutions(wa, wb, wc);
        }

        public static IEnumerable<float> Find(SquareBezierCurve a, SquareBezierCurve b)
        {
            Vector2 ua = 2 * a.Weights[1];
            Vector2 ub = 2 * b.Weights[1];
            Vector2 wa = a.Weights[0] + ua + a.Weights[2] - b.Weights[0] - ub - b.Weights[2];
            Vector2 wb = -(2 * a.Weights[0] + ua + 2 * b.Weights[0] + ub);
            Vector2 wc = a.Weights[0] - b.Weights[0];
            return QuadraticSolutions(wa, wb, wc);
        }

        private static IEnumerable<float> QuadraticSolutions(Vector2 wa, Vector2 wb, Vector2 wc)
        {
            var xsolutions = Equation.SolveQuadratic(wa.X, wb.X, wc.X);
            var ysolutions = Equation.SolveQuadratic(wa.Y, wb.Y, wc.Y);
            var solutions = xsolutions.Zip(ysolutions, (t1, t2) => (T1: t1, T2: t2));
            return from solution in solutions
                   where SolutionIsCorrect(solution.T1, solution.T2)
                   select solution.T1;
        }

        private static bool SolutionIsCorrect(float t1, float t2) => t1 == t2 && BezierHelper.CheckT(t1);
    }
}
