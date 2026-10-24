using RobotSimulator.Domain;

namespace RobotSimulator.Domain.Tests;

public class Task001Tests
{
    [Fact]
    public void Tabletop_Allows_Valid_Position_Within_Bounds()
    {
        Assert.True(Tabletop.IsValidPosition(0, 0));
        Assert.True(Tabletop.IsValidPosition(4, 4));
        Assert.True(Tabletop.IsValidPosition(2, 3));
    }

    [Fact]
    public void Tabletop_Rejects_Out_Of_Bounds_Position()
    {
        Assert.False(Tabletop.IsValidPosition(-1, 0));
        Assert.False(Tabletop.IsValidPosition(5, 0));
        Assert.False(Tabletop.IsValidPosition(0, -1));
        Assert.False(Tabletop.IsValidPosition(0, 5));
    }

    [Fact]
    public void Robot_Starts_Unplaced()
    {
        var robot = new Robot();

        Assert.False(robot.IsPlaced);
        Assert.Null(robot.Facing);
    }

    [Fact]
    public void Valid_Place_Sets_State()
    {
        var robot = new Robot();

        var result = robot.Place(0, 0, Direction.North);

        Assert.True(result);
        Assert.True(robot.IsPlaced);
        Assert.Equal(0, robot.X);
        Assert.Equal(0, robot.Y);
        Assert.Equal(Direction.North, robot.Facing);
    }

    [Fact]
    public void Invalid_Place_Outside_Board_Is_Ignored()
    {
        var robot = new Robot();

        var result = robot.Place(5, 0, Direction.North);

        Assert.False(result);
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Turn_Left_Rotates_Through_The_Full_Cycle()
    {
        var robot = new Robot();
        robot.Place(1, 1, Direction.North);

        Assert.True(robot.TurnLeft());
        Assert.Equal(Direction.West, robot.Facing);

        Assert.True(robot.TurnLeft());
        Assert.Equal(Direction.South, robot.Facing);

        Assert.True(robot.TurnLeft());
        Assert.Equal(Direction.East, robot.Facing);

        Assert.True(robot.TurnLeft());
        Assert.Equal(Direction.North, robot.Facing);
    }

    [Fact]
    public void Turn_Right_Rotates_Through_The_Full_Cycle()
    {
        var robot = new Robot();
        robot.Place(1, 1, Direction.North);

        Assert.True(robot.TurnRight());
        Assert.Equal(Direction.East, robot.Facing);

        Assert.True(robot.TurnRight());
        Assert.Equal(Direction.South, robot.Facing);

        Assert.True(robot.TurnRight());
        Assert.Equal(Direction.West, robot.Facing);

        Assert.True(robot.TurnRight());
        Assert.Equal(Direction.North, robot.Facing);
    }

    [Fact]
    public void Turn_Before_Place_Is_Ignored()
    {
        var robot = new Robot();

        Assert.False(robot.TurnLeft());
        Assert.False(robot.TurnRight());
        Assert.False(robot.IsPlaced);
    }

    [Fact]
    public void Move_Inside_Bounds_Updates_Position()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.North);

        var result = robot.Move();

        Assert.True(result);
        Assert.Equal(0, robot.X);
        Assert.Equal(1, robot.Y);
    }

    [Fact]
    public void Move_Off_The_North_Edge_Is_Ignored()
    {
        var robot = new Robot();
        robot.Place(0, 4, Direction.North);

        var result = robot.Move();

        Assert.False(result);
        Assert.Equal(0, robot.X);
        Assert.Equal(4, robot.Y);
    }

    [Fact]
    public void Move_Off_The_South_Edge_Is_Ignored()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.South);

        var result = robot.Move();

        Assert.False(result);
        Assert.Equal(0, robot.X);
        Assert.Equal(0, robot.Y);
    }

    [Fact]
    public void Move_Off_The_East_Edge_Is_Ignored()
    {
        var robot = new Robot();
        robot.Place(4, 0, Direction.East);

        var result = robot.Move();

        Assert.False(result);
        Assert.Equal(4, robot.X);
        Assert.Equal(0, robot.Y);
    }

    [Fact]
    public void Move_Off_The_West_Edge_Is_Ignored()
    {
        var robot = new Robot();
        robot.Place(0, 0, Direction.West);

        var result = robot.Move();

        Assert.False(result);
        Assert.Equal(0, robot.X);
        Assert.Equal(0, robot.Y);
    }

    [Fact]
    public void Report_Before_Place_Returns_Null()
    {
        var robot = new Robot();

        Assert.Null(robot.Report());
    }

    [Fact]
    public void Report_After_Place_Formats_Output_Correctly()
    {
        var robot = new Robot();
        robot.Place(2, 3, Direction.East);

        var result = robot.Report();

        Assert.Equal("2,3,EAST", result);
    }
}
