using System;
using System.Collections.Generic;

namespace Bezier
{
    class DistanceComparer : IComparer<Vector2>
    {
        private readonly Vector2 origin;

        public DistanceComparer(Vector2 origin) => this.origin = origin;

        public int Compare(Vector2 a, Vector2 b)
        {
            var d1 = (a - origin).SqrMagnitude;
            var d2 = (b - origin).SqrMagnitude;
            return d1.CompareTo(d2);
        }
    }

    struct Line
    {
        public Vector2 From { get; }

        public Vector2 To { get; }

        public Line(Vector2 from, Vector2 to) => (From, To) = (from, to);
    }

    public struct Rectangle
    {
        public Vector2 LowerLeft { get; }

        public Vector2 UpperRight { get; }

        public Vector2 UpperLeft => new Vector2(UpperRight.X - Width, UpperRight.Y);

        public float Width => UpperRight.X - LowerLeft.X;

        public float Height => LowerLeft.Y - UpperRight.Y;

        public Rectangle(Vector2 lowerLeft, Vector2 upperRight) => (LowerLeft, UpperRight) = (lowerLeft, upperRight);

        public bool Overlaps(Rectangle other) =>
            LowerLeft.Y >= other.UpperRight.Y && UpperRight.Y <= other.LowerLeft.Y &&
            UpperRight.X >= other.LowerLeft.X && LowerLeft.X <= other.UpperRight.X;
    }

    public struct Vector2
    {
        public static Vector2 Interpolate(Vector2 from, Vector2 to, float t) => from + (to - from) * t;

        public static readonly Vector2 Up = new Vector2(0, 1);

        public static readonly Vector2 Down = new Vector2(0, -1);

        public static readonly Vector2 Left = new Vector2(-1, 0);

        public static readonly Vector2 Right = new Vector2(1, 0);

        public float X { get; }

        public float Y { get; }

        public Vector2(float x, float y) => (X, Y) = (x, y);

        public float SqrDistance(Vector2 a) => (this - a).SqrMagnitude;

        public float SqrMagnitude => X * X + Y * Y;

        public override bool Equals(object o)
        {
            if (o is Vector2 v)
                return this == v;
            else
                return false;
        }

        public override string ToString() => $"({X}, {Y})";

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) => a + (-b);

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);

        public static Vector2 operator *(Vector2 a, float n) => new Vector2(a.X * n, a.Y * n);

        public static Vector2 operator *(float n, Vector2 a) => a * n;

        public static Vector2 operator /(Vector2 a, float n) => new Vector2(a.X / n, a.Y / n);

        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
    }
}