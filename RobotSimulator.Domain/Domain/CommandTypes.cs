namespace RobotSimulator.Domain;

public abstract record Command;

public sealed record PlaceCommand(int X, int Y, Direction Direction) : Command;
public sealed record MoveCommand : Command;
public sealed record LeftCommand : Command;
public sealed record RightCommand : Command;
public sealed record ReportCommand : Command;
