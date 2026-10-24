# Development Guide

## Overview

This project was developed using the Spec-Driven Development (SDD) workflow:
1. review the original challenge requirements
2. define the functional specification
3. review the specification for correctness and gaps
4. produce a minimal design
5. create an implementation plan
6. implement the approved domain and command-processing logic
7. validate with focused and full automated tests
8. report findings and stop for review at each stage

## Tools and environment

- .NET 8 SDK
- C# .NET class library and console project
- xUnit test framework
- PowerShell for build and execution commands
- VS Code workspace and local files for requirements, design, planning, and verification records

## How AI was used

AI assistance was used to support the SDD workflow across the following stages:

### Requirements stage
- reviewed the challenge wording against the approved specification expectations
- identified missing explicit details and ambiguities
- distinguished assessment requirements from pending product-level decisions

### Design stage
- produced a minimal architecture consistent with the approved requirements
- separated domain logic from input/output concerns
- kept the design focused on a small console application

### Implementation stage
- created the domain layer and command processor consistent with the spec
- kept the code small and testable
- avoided adding features beyond the approved requirement set

### Testing stage
- added focused unit tests for domain and command processing behaviour
- verified official assessment examples
- ran the full project test suite and recorded the actual results

## Important design decisions and trade-offs

- Domain logic is separated from console parsing and output formatting.
- Table boundary logic is enforced in the domain model rather than only in the input layer.
- Invalid commands are treated as no-ops to match the approved requirement that they do not change robot state.
- The project remains intentionally simple and does not add frameworks, services, or unrelated abstractions.

## Correctness verification

The project was verified by executing the actual .NET test suite:

```powershell
dotnet test RobotSimulator.Domain.Tests/RobotSimulator.Domain.Tests.csproj --no-restore
```

The executed result is:
- 35 tests passed
- 0 failed
- 0 skipped

The console build was also verified with:

```powershell
dotnet build RobotSimulator.slnx
```

The project build succeeded.

## Remaining limitations

- This implementation is intentionally small and console-oriented; it does not introduce a GUI or external service layer.
- The specification leaves some product-level decisions as pending review items, but they are not treated as approved requirements.
- The current submission is aligned to the approved requirements and does not broaden scope beyond the challenge.
