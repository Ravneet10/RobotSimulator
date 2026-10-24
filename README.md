# Toy Robot Simulator

## Project overview

This project implements a small console-based Toy Robot Simulator according to the approved specification and design. It models a robot on a 5 x 5 tabletop, validates commands, ignores invalid input without changing state, and emits REPORT output in the required `X,Y,ORIENTATION` format.

The implementation is intentionally minimal and keeps pure domain logic separate from command parsing and console interaction.

## Build

From the repository root:

```powershell
dotnet build RobotSimulator.slnx
```

## Run

Use a PowerShell here-string to send commands to the console app:

```powershell
@'
PLACE 0,0,NORTH
MOVE
REPORT
'@ | dotnet run --project .\RobotSimulator.Console\RobotSimulator.Console.csproj
```

Expected output:

```text
0,1,NORTH
```

## Run in Visual Studio Code

You can run the app directly in VS Code without using a separate terminal window:

1. Open the repository in VS Code.
2. Open the integrated terminal from `Terminal > New Terminal`.
3. Run:

    ```powershell
    dotnet run --project .\RobotSimulator.Console\RobotSimulator.Console.csproj
    ```

4. Type commands one per line, for example:

    ```text
    PLACE 0,0,NORTH
    MOVE
    REPORT
    ```

5. Press `Ctrl+C` to stop the app when you are done.

For debugging, use the VS Code C# extension and start with `F5` or `Run and Debug`, selecting the console app configuration when available. This is the best option if you want to inspect variables or step through execution.

## Recommended way to run

For a simple console challenge, the best default is:

```powershell
dotnet run --project .\RobotSimulator.Console\RobotSimulator.Console.csproj
```

This is the simplest and most reliable option for local execution and automation.

If you want to debug the code step by step, use VS Code with the C# debugger. If you want the fastest execution path for a challenge submission, use `dotnet run` from the terminal.

## Tests

Run the automated test suite:

```powershell
dotnet test RobotSimulator.Domain.Tests/RobotSimulator.Domain.Tests.csproj --no-restore
```

## Supported commands and input format

Supported commands:
- `PLACE X,Y,F`
- `MOVE`
- `LEFT`
- `RIGHT`
- `REPORT`

Valid table bounds:
- X = 0..4
- Y = 0..4

Valid orientations:
- NORTH
- EAST
- SOUTH
- WEST

Rules:
- A robot must receive a valid `PLACE` before `MOVE`, `LEFT`, `RIGHT`, or `REPORT` can affect state.
- Invalid placements, malformed commands, unsupported commands, commands before placement, and boundary-crossing moves are silently ignored without changing state.
- Only a valid `REPORT` produces output, in the format `X,Y,ORIENTATION`.

## Example usage and expected output

### Example 1: basic move
Input:
```text
PLACE 0,0,NORTH
MOVE
REPORT
```

Output:
```text
0,1,NORTH
```

### Example 2: left turn
Input:
```text
PLACE 0,0,NORTH
LEFT
REPORT
```

Output:
```text
0,0,WEST
```

### Example 3: multi-step movement and turn
Input:
```text
PLACE 1,1,EAST
MOVE
MOVE
LEFT
MOVE
REPORT
```

Output:
```text
3,2,NORTH
```