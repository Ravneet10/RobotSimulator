namespace RobotSimulator.Domain;

public interface ICommandHandler
{
    bool CanHandle(Command command);
    Result<string?> Execute(Robot robot, Command command);
}
