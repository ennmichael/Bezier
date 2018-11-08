using System.Collections.Generic;

namespace Bezier
{
    static class IntersectionsExtension
    {
        private static readonly float maximumSize = 0.001f;

        // This algorithm can be much faster
        // It probably yields too many results
        public static IEnumerable<Vector2> Intersections(this ICurve a, ICurve b)
        {
            Rectangle aBoundingBox = a.BoundingBox();
            Rectangle bBoundingBox = b.BoundingBox();
            if (aBoundingBox.Overlaps(bBoundingBox))
            {
                if (aBoundingBox.Width < maximumSize && aBoundingBox.Height < maximumSize &&
                    bBoundingBox.Width < maximumSize && bBoundingBox.Height < maximumSize)
                {
                    yield return aBoundingBox.UpperRight;
                }
                else
                {
                    var (a1, a2) = a.Split(0.5f);
                    var (b1, b2) = b.Split(0.5f);
                    foreach (Vector2 intersection in a1.Intersections(b1))
                        yield return intersection;
                    foreach (Vector2 intersection in a1.Intersections(b2))
                        yield return intersection;
                    foreach (Vector2 intersection in a2.Intersections(b1))
                        yield return intersection;
                    foreach (Vector2 intersection in a2.Intersections(b2))
                        yield return intersection;
                }
            }
        }
    }
}
