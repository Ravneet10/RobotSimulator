namespace RobotSimulator.Domain;

public class ConsoleReporter
{
    public string FormatReport(string? report)
    {
        return report ?? string.Empty;
    }
}
