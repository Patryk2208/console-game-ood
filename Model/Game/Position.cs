namespace Model.Game;

public struct Position(int x, int y) : IEquatable<Position>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public bool IsSet() => X >= 0 && Y >= 0;
    public bool Equals(Position other) => X == other.X && Y == other.Y;
    public float Distance(Position other) => (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}