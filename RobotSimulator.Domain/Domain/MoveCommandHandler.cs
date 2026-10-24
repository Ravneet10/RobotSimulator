namespace RobotSimulator.Domain;

public sealed class MoveCommandHandler : ICommandHandler
{
    public bool CanHandle(Command command) => command is MoveCommand;

    public Result<string?> Execute(Robot robot, Command command)
    {
        return robot.Move()
            ? Result<string?>.Success(null)
            : Result<string?>.Failure("Robot cannot move in that direction.");
    }
}
