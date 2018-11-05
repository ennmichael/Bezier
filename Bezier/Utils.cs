using System;
using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    public class ApproximateEqualityComparer : IEqualityComparer<float>
    {
        public bool Equals(float x, float y) => Math.Abs(x - y) <= Utils.Epsilon;

        public int GetHashCode(float x) => x.GetHashCode();
    }

    static class Utils
    {
        public static readonly float Epsilon = 1e-5f;

        public static IEnumerable<(T Value, int Index)> ValuesAndIndices<T>(IEnumerable<T> values) =>
            values.Select((v, i) => (v, i));

        public static bool VeryClose(float a, float b) => Math.Abs(a - b) <= Epsilon;

        public static bool CheckT(float t) => t >= 0.0f && t <= 1.0f;

        public static (float Min, float Max) MinMax(float a, float b) => (a > b) ? (a, b) : (b, a);

        public static IEnumerable<float> Common(IEnumerable<float> a, IEnumerable<float> b) =>
            a.Intersect(b, new ApproximateEqualityComparer());

        public static IEnumerable<float> CommonTs(IEnumerable<float> ts1, IEnumerable<float> ts2) => Common(ts1, ts2).Where(CheckT);
    }
}
