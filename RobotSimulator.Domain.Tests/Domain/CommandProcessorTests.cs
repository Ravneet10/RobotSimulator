using RobotSimulator.Domain;

namespace RobotSimulator.Domain.Tests;

public class CommandProcessorTests
{
    [Fact]
    public void Move_Before_First_Place_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        var result = processor.Process("MOVE");

        Assert.Null(result);
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Left_Before_First_Place_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        var result = processor.Process("LEFT");

        Assert.Null(result);
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Right_Before_First_Place_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        var result = processor.Process("RIGHT");

        Assert.Null(result);
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Report_Before_First_Place_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        var result = processor.Process("REPORT");

        Assert.Null(result);
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Valid_Place_Then_Report_Produces_Required_Output()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,0,NORTH");
        var result = processor.Process("REPORT");

        Assert.Equal("0,0,NORTH", result);
    }

    [Fact]
    public void Process_Example_One()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,0,NORTH");
        processor.Process("MOVE");
        var result = processor.Process("REPORT");

        Assert.Equal("0,1,NORTH", result);
    }

    [Fact]
    public void Process_Example_Two()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,0,NORTH");
        processor.Process("LEFT");
        var result = processor.Process("REPORT");

        Assert.Equal("0,0,WEST", result);
    }

    [Fact]
    public void Process_Example_Three()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 1,1,EAST");
        processor.Process("MOVE");
        processor.Process("MOVE");
        processor.Process("LEFT");
        processor.Process("MOVE");
        var result = processor.Process("REPORT");

        Assert.Equal("3,2,NORTH", result);
    }

    [Fact]
    public void Invalid_Place_Is_Ignored_And_Subsequent_Commands_Still_Apply()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 5,0,NORTH");
        processor.Process("PLACE 0,0,NORTH");
        processor.Process("MOVE");
        var result = processor.Process("REPORT");

        Assert.Equal("0,1,NORTH", result);
    }

    [Fact]
    public void Unknown_Command_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,0,NORTH");
        processor.Process("JUMP");
        var result = processor.Process("REPORT");

        Assert.Equal("0,0,NORTH", result);
    }

    [Fact]
    public void Malformed_Place_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,0");
        processor.Process("PLACE 0,0,NORTH");
        var result = processor.Process("REPORT");

        Assert.Equal("0,0,NORTH", result);
    }

    [Fact]
    public void Empty_Command_Is_Ignored()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process(" ");
        processor.Process("PLACE 0,0,NORTH");
        var result = processor.Process("REPORT");

        Assert.Equal("0,0,NORTH", result);
    }

    [Fact]
    public void Console_Input_Reader_Processes_Command_Lines()
    {
        var reader = new ConsoleInputReader();

        var commands = reader.ReadCommands("PLACE 0,0,NORTH\nMOVE\nREPORT");

        Assert.Equal(new[] { "PLACE 0,0,NORTH", "MOVE", "REPORT" }, commands);
    }

    [Fact]
    public void Console_Reporter_Format_Is_Stable()
    {
        var reporter = new ConsoleReporter();

        Assert.Equal("0,0,NORTH", reporter.FormatReport("0,0,NORTH"));
        Assert.Equal(string.Empty, reporter.FormatReport(null));
    }

    [Fact]
    public void Valid_Place_At_Each_Corner_Produces_Expected_State()
    {
        var cases = new[]
        {
            ("PLACE 0,0,NORTH", "0,0,NORTH"),
            ("PLACE 0,4,EAST", "0,4,EAST"),
            ("PLACE 4,0,SOUTH", "4,0,SOUTH"),
            ("PLACE 4,4,WEST", "4,4,WEST")
        };

        foreach (var (command, expected) in cases)
        {
            var robot = new Robot();
            var processor = new CommandProcessor(robot);

            processor.Process(command);
            var result = processor.Process("REPORT");

            Assert.Equal(expected, result);
        }
    }

    [Fact]
    public void Repeated_Valid_Place_Overwrites_Previous_State()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 1,1,NORTH");
        processor.Process("PLACE 2,2,SOUTH");
        var result = processor.Process("REPORT");

        Assert.Equal("2,2,SOUTH", result);
    }

    [Fact]
    public void Blocked_Move_Preserves_Position_And_Allows_Subsequent_Valid_Command()
    {
        var robot = new Robot();
        var processor = new CommandProcessor(robot);

        processor.Process("PLACE 0,4,NORTH");
        processor.Process("MOVE");
        processor.Process("LEFT");
        var result = processor.Process("REPORT");

        Assert.Equal("0,4,WEST", result);
    }

    [Fact]
    public void All_Four_Orientations_Are_Supported()
    {
        var cases = new[]
        {
            ("PLACE 1,1,NORTH", "1,1,NORTH"),
            ("PLACE 1,1,EAST", "1,1,EAST"),
            ("PLACE 1,1,SOUTH", "1,1,SOUTH"),
            ("PLACE 1,1,WEST", "1,1,WEST")
        };

        foreach (var (command, expected) in cases)
        {
            var robot = new Robot();
            var processor = new CommandProcessor(robot);

            processor.Process(command);
            var result = processor.Process("REPORT");

            Assert.Equal(expected, result);
        }
    }

    [Fact]
    public void All_Four_Boundaries_Reject_Off_Table_Moves()
    {
        var cases = new[]
        {
            ("PLACE 0,4,NORTH", "MOVE", "0,4,NORTH"),
            ("PLACE 0,0,SOUTH", "MOVE", "0,0,SOUTH"),
            ("PLACE 4,0,EAST", "MOVE", "4,0,EAST"),
            ("PLACE 0,0,WEST", "MOVE", "0,0,WEST")
        };

        foreach (var (place, move, expected) in cases)
        {
            var robot = new Robot();
            var processor = new CommandProcessor(robot);

            processor.Process(place);
            processor.Process(move);
            var result = processor.Process("REPORT");

            Assert.Equal(expected, result);
        }
    }
}
