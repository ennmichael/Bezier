using System.Collections.Generic;

namespace Bezier
{
    interface IBezierCurve
    {
        IList<Vector2> Weights { get; }

        Vector2 Point(float t);
    }
}
