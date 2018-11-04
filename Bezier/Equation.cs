using System;
using System.Collections.Generic;

namespace Bezier
{
    public static class Equation
    {
        public static IEnumerable<float> SolveCubic(float a, float b, float c, float d)
        {
            // a * x3 + b * x2 + c * x + d
            if (Utils.VeryClose(a, 0.0f))
            {
                foreach (float x in SolveQuadratic(b, c, d))
                    yield return x;
            }
            else
            {
                float newA = b / a;
                float newB = c / a;
                float newC = d / a;
                float newA2 = newA * newA;
                float p = newB - newA2 / 3;
                float q = 2 * newA2 * newA / 27 - newA * newB / 3 + newC;
                bool pIsZero = Utils.VeryClose(p, 0.0f);
                bool qIsZero = Utils.VeryClose(q, 0.0f);
                if (pIsZero && qIsZero)
                {
                    yield return 0.0f;
                }
                else if (pIsZero)
                {
                    yield return Cuberoot(-q);
                }
                else if (qIsZero)
                {
                    yield return Cuberoot(-p);
                }
                else
                {
                    float qHalf = q / 2.0f;
                    float qHalfSqr = qHalf * qHalf;
                    float pThird = p / 3.0f;
                    float aThird = newA / 3.0f;
                    float D = qHalfSqr + pThird * pThird * pThird;
                    if (Utils.VeryClose(D, 0.0f))
                    {
                        float qHalfCuberoot = Cuberoot(qHalf);
                        yield return -2 * qHalfCuberoot - aThird;
                        yield return qHalfCuberoot - aThird;
                    }
                    else if (D > 0.0f)
                    {
                        var DSqrt = (float)Math.Sqrt(D);
                        yield return Cuberoot(-qHalf + DSqrt) + Cuberoot(-qHalf - DSqrt) - aThird;
                    }
                    else
                    {
                        float Z = (float)Math.Sqrt(qHalfSqr - D);
                        float zCuberoot = Cuberoot(Z);
                        float zCuberootDoubled = 2.0f * zCuberoot;
                        var alpha = (float)Math.Acos(-qHalf / Z);
                        yield return zCuberootDoubled * (float)Math.Cos(alpha / 3.0f) - aThird;
                        yield return zCuberootDoubled * (float)Math.Cos((alpha + 2.0f * Math.PI) / 3.0f) - aThird;
                        yield return zCuberootDoubled * (float)Math.Cos((alpha + 4.0f * Math.PI) / 3.0f) - aThird;
                    }
                }
            }
        }

        private static float Cuberoot(float x)
        {
            double power = 1.0 / 3.0;
            double result = (x < 0) ? -Math.Pow(-x, power) : Math.Pow(x, power);
            return (float)result;
        }

        public static IEnumerable<float> SolveQuadratic(float a, float b, float c)
        {
            if (a == 0)
            {
                if (b != 0)
                {
                    float? result = SolveLinear(b, c);
                    if (result.HasValue)
                        yield return result.Value;
                }
            }
            else
            {
                float D = b * b - 4 * a * c;
                if (D == 0)
                {
                    yield return -b / (2 * a);
                }
                else if (D > 0)
                {
                    float doubleA = 2 * a;
                    float sqrtD = (float)Math.Sqrt(D);
                    yield return (-b + sqrtD) / doubleA;
                    yield return (-b - sqrtD) / doubleA;
                }
            }
        }

        public static float? SolveLinear(float a, float b)
        {
            if (a == 0)
                return null;
            return -b / a;
        }
    }
}
