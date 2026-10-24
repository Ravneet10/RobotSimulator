using RobotSimulator.Domain;

var robot = new Robot();
var processor = new CommandProcessor(robot);
var reader = new ConsoleInputReader();
var reporter = new ConsoleReporter();

string? line;
while ((line = Console.ReadLine()) is not null)
{
    var commands = reader.ReadCommands(line);
    foreach (var command in commands)
    {
        var result = processor.Process(command);
        if (!string.IsNullOrEmpty(result))
        {
            Console.WriteLine(reporter.FormatReport(result));
        }
    }
}
