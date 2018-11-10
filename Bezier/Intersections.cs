using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    static class IntersectionsExtension
    {
        private static readonly float maximumSize = 2.0f;

        public static IEnumerable<Vector2> Intersections(this ICurve a, ICurve b)
        {
            var intersections = FindIntersections(a, b);
            return CleanIntersections(intersections);
        }

        private static IEnumerable<Vector2> FindIntersections(ICurve a, ICurve b)
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

        private static IEnumerable<Vector2> CleanIntersections(IEnumerable<Vector2> intersections)
        {
            var cleanIntersections = new List<Vector2>();
            foreach (Vector2 intersection in intersections)
            {
                if (!cleanIntersections.Any(i => i.SqrDistance(intersection) <= 70.0f))
                {
                    cleanIntersections.Add(intersection);
                    yield return intersection;
                }
            }
        }
    }
}
