namespace RobotSimulator.Domain;

public sealed class PlaceCommandHandler : ICommandHandler
{
    public bool CanHandle(Command command) => command is PlaceCommand;

    public Result<string?> Execute(Robot robot, Command command)
    {
        if (command is not PlaceCommand placeCommand)
        {
            return Result<string?>.Failure("Invalid place command.");
        }

        return robot.Place(placeCommand.X, placeCommand.Y, placeCommand.Direction)
            ? Result<string?>.Success(null)
            : Result<string?>.Failure("Placement is outside the table bounds.");
    }
}
