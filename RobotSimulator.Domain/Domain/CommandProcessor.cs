namespace RobotSimulator.Domain;

public class CommandProcessor
{
    private readonly Robot _robot;
    private readonly IReadOnlyList<ICommandHandler> _handlers;

    public CommandProcessor(Robot robot, IEnumerable<ICommandHandler>? handlers = null)
    {
        _robot = robot ?? throw new ArgumentNullException(nameof(robot));
        _handlers = (handlers ?? CreateDefaultHandlers()).ToList();
    }

    public string? Process(string? commandText)
    {
        var command = CommandParser.Parse(commandText);
        if (command is null)
        {
            return null;
        }

        foreach (var handler in _handlers)
        {
            if (handler.CanHandle(command))
            {
                var result = handler.Execute(_robot, command);
                return result.IsSuccess ? result.Value : null;
            }
        }

        return null;
    }

    private static IReadOnlyList<ICommandHandler> CreateDefaultHandlers()
    {
        return new ICommandHandler[]
        {
            new MoveCommandHandler(),
            new LeftCommandHandler(),
            new RightCommandHandler(),
            new ReportCommandHandler(),
            new PlaceCommandHandler()
        };
    }
}
