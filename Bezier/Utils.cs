using System;
using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    static class Utils
    {
        public static readonly float Epsilon = 1e-5f;

        public static IEnumerable<(T Value, int Index)> ValuesAndIndices<T>(IEnumerable<T> values) =>
            values.Select((v, i) => (v, i));

        public static bool VeryClose(float a, float b) => Math.Abs(a - b) <= Epsilon;
    }
}
