namespace RobotSimulator.Domain;

public sealed class LeftCommandHandler : ICommandHandler
{
    public bool CanHandle(Command command) => command is LeftCommand;

    public Result<string?> Execute(Robot robot, Command command)
    {
        return robot.TurnLeft()
            ? Result<string?>.Success(null)
            : Result<string?>.Failure("Robot is not placed.");
    }
}
