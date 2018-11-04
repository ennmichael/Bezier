using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    static class Utils
    {
        public static IEnumerable<(T Value, int Index)> ValuesAndIndices<T>(IEnumerable<T> values) =>
            values.Select((v, i) => (v, i));
    }
}
