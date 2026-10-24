namespace RobotSimulator.Domain;

public class ConsoleInputReader
{
    public IEnumerable<string> ReadCommands(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Enumerable.Empty<string>();
        }

        return input
            .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line));
    }
}
