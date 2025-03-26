using System.ComponentModel.Design;

namespace RPG_ood;

public struct Position(int x, int y) : IEquatable<Position>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public bool IsSet() => X >= 0 && Y >= 0;
    public bool Equals(Position other) => X == other.X && Y == other.Y;
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}