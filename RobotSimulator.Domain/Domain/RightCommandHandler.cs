namespace RobotSimulator.Domain;

public sealed class RightCommandHandler : ICommandHandler
{
    public bool CanHandle(Command command) => command is RightCommand;

    public Result<string?> Execute(Robot robot, Command command)
    {
        return robot.TurnRight()
            ? Result<string?>.Success(null)
            : Result<string?>.Failure("Robot is not placed.");
    }
}
