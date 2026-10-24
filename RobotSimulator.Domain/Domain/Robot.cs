namespace RobotSimulator.Domain;

public class Robot
{
    public bool IsPlaced { get; private set; }
    public Position Position { get; private set; }
    public Direction? Facing { get; private set; }
    public int X => Position.X;
    public int Y => Position.Y;

    public bool Place(int x, int y, Direction direction)
    {
        if (!Enum.IsDefined(typeof(Direction), direction) || !Tabletop.IsValidPosition(x, y))
        {
            return false;
        }

        Position = new Position(x, y);
        Facing = direction;
        IsPlaced = true;
        return true;
    }

    public bool TurnLeft()
    {
        if (!IsPlaced || Facing is null)
        {
            return false;
        }

        Facing = Facing switch
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => Facing
        };

        return true;
    }

    public bool TurnRight()
    {
        if (!IsPlaced || Facing is null)
        {
            return false;
        }

        Facing = Facing switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => Facing
        };

        return true;
    }

    public bool Move()
    {
        if (!IsPlaced || Facing is null)
        {
            return false;
        }

        var nextX = Position.X;
        var nextY = Position.Y;

        switch (Facing)
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

        if (!Tabletop.IsValidPosition(nextX, nextY))
        {
            return false;
        }

        Position = new Position(nextX, nextY);
        return true;
    }

    public string? Report()
    {
        if (!IsPlaced || Facing is null)
        {
            return null;
        }

        var direction = Facing.Value;
        return $"{Position.X},{Position.Y},{direction.ToString().ToUpperInvariant()}";
    }
}
