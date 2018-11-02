namespace Bezier
{
    struct Line
    {
        public Vector2 From { get; }

        public Vector2 To { get; }

        public Line(Vector2 from, Vector2 to) => (From, To) = (from, to);

        public (Line First, Line Second) Ratio(float t)
        {
            Vector2 a = From + (To - From) * t;
            return (new Line(From, a), new Line(a, To));
        }
    }

    struct Vector2
    {
        public static readonly Vector2 Up = new Vector2(0, 1);

        public static readonly Vector2 Down = new Vector2(0, -1);

        public static readonly Vector2 Left = new Vector2(-1, 0);

        public static readonly Vector2 Right = new Vector2(1, 0);

        public float X { get; }

        public float Y { get; }

        public Vector2(float x, float y) => (X, Y) = (x, y);

        public static float SqrDistance(Vector2 a, Vector2 b) => (b - a).SqrMagnitude;

        public float SqrMagnitude => X * X + Y * Y;

        public override bool Equals(object o)
        {
            if (o is Vector2 v)
                return this == v;
            else
                return false;
        }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) => a + (-b);

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);

        public static Vector2 operator *(Vector2 a, float n) => new Vector2(a.X * n, a.Y * n);

        public static Vector2 operator *(float n, Vector2 a) => a * n;

        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
    }
}