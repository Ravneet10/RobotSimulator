namespace RobotSimulator.Domain;

public sealed class ReportCommandHandler : ICommandHandler
{
    public bool CanHandle(Command command) => command is ReportCommand;

    public Result<string?> Execute(Robot robot, Command command)
    {
        var report = robot.Report();

        return report is null
            ? Result<string?>.Failure("Robot is not placed.")
            : Result<string?>.Success(report);
    }
}
