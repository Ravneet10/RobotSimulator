namespace RobotSimulator.Domain;

public static class Tabletop
{
    public const int MinX = 0;
    public const int MaxX = 4;
    public const int MinY = 0;
    public const int MaxY = 4;

    public static bool IsValidPosition(int x, int y)
    {
        return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }

    public static bool CanMoveFrom(int x, int y, Direction direction)
    {
        int nextX = x;
        int nextY = y;

        switch (direction)
        {
            case Direction.North:
                nextY += 1;
                break;
            case Direction.South:
                nextY -= 1;
                break;
            case Direction.East:
                nextX += 1;
                break;
            case Direction.West:
                nextX -= 1;
                break;
            default:
                return false;
        }

        return IsValidPosition(nextX, nextY);
    }
}
