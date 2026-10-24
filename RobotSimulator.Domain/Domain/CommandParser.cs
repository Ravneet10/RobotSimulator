namespace RobotSimulator.Domain;

public static class CommandParser
{
    public static Command? Parse(string? rawCommand)
    {
        if (string.IsNullOrWhiteSpace(rawCommand))
        {
            return null;
        }

        var command = rawCommand.Trim();

        return command.ToUpperInvariant() switch
        {
            "MOVE" => new MoveCommand(),
            "LEFT" => new LeftCommand(),
            "RIGHT" => new RightCommand(),
            "REPORT" => new ReportCommand(),
            _ when command.StartsWith("PLACE ", StringComparison.OrdinalIgnoreCase) => ParsePlace(command),
            _ => null
        };
    }

    private static Command? ParsePlace(string command)
    {
        var placeValue = command[6..].Trim();
        var tokens = placeValue.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length != 3)
        {
            return null;
        }

        if (!int.TryParse(tokens[0], out var x) || !int.TryParse(tokens[1], out var y))
        {
            return null;
        }

        if (!Enum.TryParse<Direction>(tokens[2], true, out var direction))
        {
            return null;
        }

        return new PlaceCommand(x, y, direction);
    }
}
