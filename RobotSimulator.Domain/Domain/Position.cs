namespace RobotSimulator.Domain;

public readonly record struct Position(int X, int Y)
{
    public bool IsWithinBounds() => Tabletop.IsValidPosition(X, Y);
}
