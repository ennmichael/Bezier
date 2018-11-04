using System;
using System.Collections.Generic;

namespace Bezier
{
    static class Equation
    {
        public static IEnumerable<float> SolveQuadratic(float a, float b, float c)
        {
            float D = b * b - 4 * a * c;
            if (D == 0)
            {
                yield return -b / 2 * a;
            }
            else if (D > 0)
            {
                float doubleA = 2 * a;
                float sqrtD = (float)Math.Sqrt(D);
                yield return (-b + sqrtD) / doubleA;
                yield return (-b - sqrtD) / doubleA;
            }
        }

        public static IEnumerable<float> SolveCubic(float a, float b, float c, float d)
        {
            yield return 0.0f;
        }
    }
}
