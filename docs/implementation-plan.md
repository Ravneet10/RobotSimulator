# Toy Robot Simulator Implementation Plan

## 1. Purpose and Scope

This implementation plan translates the approved requirements and design into a minimal, testable delivery plan for the C# .NET Toy Robot Simulator.

The plan is intentionally implementation-focused but remains aligned to the approved specification and design. It defines:
- small, independently verifiable tasks
- dependency ordering
- test coverage aligned to the specification
- explicit completion criteria for each task
- traceability from requirements to work items and tests

## 2. Constraints and Non-Goals

- Do not change the approved requirements.
- Do not add speculative features or user interfaces beyond the console command flow.
- Keep the implementation small, maintainable, and testable.
- Use the actual .NET solution structure that was implemented for this assessment.

## 3. Proposed Delivery Structure

The implementation uses the following C# .NET project layout:

- `RobotSimulator.Domain/Domain/` for domain models and rules
- `RobotSimulator.Domain/Console/` for input/output adapters
- `RobotSimulator.Console/Program.cs` for the console entry point
- `RobotSimulator.Domain.Tests/Domain/` for automated tests
- `test-data/` for reusable command sequences and expected outputs
- `docs/` for requirements, design, and validation documentation

This structure reflects the final implemented architecture and should be treated as the canonical project layout for this solution.

## 4. Task Breakdown

### TASK-001: Define domain primitives and validation constants

- Objective: Establish the core types and constants for the board, valid orientations, and robot state.
- Applicable requirements: FR-001, FR-002, BR-001, BR-002, BR-009
- Applicable acceptance criteria: AC-001, AC-005, AC-006, AC-013
- Expected affected files/components:
  - `RobotSimulator.Domain/Domain/Tabletop.cs`
  - `RobotSimulator.Domain/Domain/Direction.cs`
  - `RobotSimulator.Domain/Domain/Robot.cs`
- Dependencies: none
- Completion criterion:
  - Valid board bounds are defined as X = 0..4 and Y = 0..4.
  - Valid directions are limited to NORTH, EAST, SOUTH, WEST.
  - Core state fields are explicit and testable.

### TASK-002: Implement tabletop boundary logic

- Objective: Encapsulate all board validity and movement-in-bounds checks.
- Applicable requirements: FR-001, FR-006, BR-001, BR-004, BR-009
- Applicable acceptance criteria: AC-007, AC-008, AC-013
- Expected affected files/components:
  - `RobotSimulator.Domain/Domain/Tabletop.cs`
- Dependencies: TASK-001
- Completion criterion:
  - A valid move is allowed only when the next coordinate remains within the board.
  - Any move from a boundary that would exceed the board is rejected and leaves state unchanged.
  - All four edges and all four corners are covered by tests.

### TASK-003: Implement robot movement and rotation rules

- Objective: Encode state transitions for PLACE, MOVE, LEFT, and RIGHT.
- Applicable requirements: FR-003, FR-004, FR-005, FR-006, FR-007, FR-008, FR-011, FR-012, BR-003, BR-005, BR-006
- Applicable acceptance criteria: AC-001, AC-002, AC-003, AC-004, AC-005, AC-006, AC-007, AC-008, AC-011, AC-013
- Expected affected files/components:
  - `RobotSimulator.Domain/Domain/Robot.cs`
  - `RobotSimulator.Domain/Domain/Direction.cs`
- Dependencies: TASK-001, TASK-002
- Completion criterion:
  - Robot starts unplaced.
  - Valid PLACE sets X, Y, and orientation.
  - Invalid PLACE does nothing.
  - MOVE increments one step in the current facing direction only when valid.
  - LEFT and RIGHT rotate correctly with wraparound.
  - Repeated valid PLACE commands overwrite previous state.

### TASK-004: Implement command parsing and validation

- Objective: Convert raw input into command objects or equivalent typed commands and validate syntax.
- Applicable requirements: FR-004, FR-005, FR-010, BR-006, BR-007, BR-008
- Applicable acceptance criteria: AC-002, AC-003, AC-004, AC-009, AC-010, AC-012
- Expected affected files/components:
  - `RobotSimulator.Domain/Domain/CommandParser.cs`
  - `RobotSimulator.Domain/Domain/CommandProcessor.cs`
  - `RobotSimulator.Domain/Domain/*CommandHandler.cs`
- Dependencies: TASK-001, TASK-003
- Completion criterion:
  - Supported commands are recognized: PLACE, MOVE, LEFT, RIGHT, REPORT.
  - Unsupported commands are ignored without state change.
  - Malformed PLACE commands are ignored.
  - Invalid coordinate or orientation values are ignored.

### TASK-005: Implement command processing workflow and REPORT output

- Objective: Sequence commands in order and apply valid operations only.
- Applicable requirements: FR-003, FR-009, FR-010, FR-011, FR-013, BR-003, BR-006, BR-008
- Applicable acceptance criteria: AC-001, AC-009, AC-010, AC-011, AC-012
- Expected affected files/components:
  - `RobotSimulator.Domain/Domain/CommandProcessor.cs`
  - `RobotSimulator.Domain/Console/ConsoleReporter.cs`
- Dependencies: TASK-003, TASK-004
- Completion criterion:
  - Commands before the first valid PLACE are ignored.
  - REPORT before placement yields no valid report output.
  - REPORT after placement outputs `X,Y,ORIENTATION`.
  - Valid commands mutate state in order.
  - Invalid commands are no-ops.

### TASK-006: Build console input/output adapter

- Objective: Connect user input to the command processor and route outputs to the console.
- Applicable requirements: FR-009, FR-013, BR-008
- Applicable acceptance criteria: AC-009, AC-011
- Expected affected files/components:
  - `RobotSimulator.Domain/Console/ConsoleInputReader.cs`
  - `RobotSimulator.Domain/Console/ConsoleReporter.cs`
  - `RobotSimulator.Console/Program.cs`
- Dependencies: TASK-004, TASK-005
- Completion criterion:
  - Input can be processed from a sequence of commands.
  - Valid REPORT outputs are written exactly as `X,Y,ORIENTATION`.
  - Empty or incomplete input is handled as an empty sequence or ignored command.

### TASK-007: Unit and integration test suite

- Objective: Validate domain logic and command sequencing using test data.
- Applicable requirements: All functional and acceptance requirements
- Applicable acceptance criteria: All
- Expected affected files/components:
  - `RobotSimulator.Domain.Tests/Domain/*.cs`
  - `test-data/test-matrix.md`
- Dependencies: TASK-001 through TASK-006
- Completion criterion:
  - All required assessment examples pass.
  - All boundary, invalid-input, rotation, and command-order scenarios pass.
  - No failing tests remain for the approved specification.

### TASK-008: Final validation and sign-off

- Objective: Confirm that implementation satisfies the approved specification before approval for release.
- Applicable requirements: All
- Applicable acceptance criteria: All
- Expected affected files/components:
  - validation notes, review checklist, final assessment evidence
- Dependencies: TASK-007
- Completion criterion:
  - Traceability review passes.
  - All official assessment examples succeed.
  - No approved requirement is violated.

## 5. Dependency Order and Recommended Execution Sequence

1. TASK-001: Domain primitives and constants
2. TASK-002: Tabletop boundary logic
3. TASK-003: Robot movement and rotation rules
4. TASK-004: Parsing and validation
5. TASK-005: Command processing and REPORT output
6. TASK-006: Console I/O adapter
7. TASK-007: Unit and integration test suite
8. TASK-008: Review and sign-off

This order keeps the domain model stable before input-processing logic is introduced, while ensuring the command processor is only built once the robot and board behaviors are already validated.

## 6. Test Matrix

The matrix below covers required assessment scenarios, boundary conditions, invalid input, and command sequences.

### 6.1 Unit tests: domain logic

| Test ID | Scenario | Input / Setup | Expected Result |
| --- | --- | --- | --- |
| UT-001 | Initial unplaced state | fresh robot | isPlaced = false |
| UT-002 | Report before placement | unplaced robot, REPORT | no valid report output; no state change |
| UT-003 | Valid place in corner | place 0,0,NORTH | state = placed at 0,0,NORTH |
| UT-004 | Valid place at opposite corner | place 4,4,SOUTH | state = placed at 4,4,SOUTH |
| UT-005 | Invalid X coordinate | place -1,0,NORTH | ignored |
| UT-006 | Invalid Y coordinate | place 0,5,NORTH | ignored |
| UT-007 | Invalid orientation | place 0,0,UP | ignored |
| UT-008 | LEFT from NORTH | placed at 0,0,NORTH; LEFT | facing WEST |
| UT-009 | RIGHT from NORTH | placed at 0,0,NORTH; RIGHT | facing EAST |
| UT-010 | LEFT wraparound | NORTH -> LEFT -> WEST -> LEFT -> SOUTH -> LEFT -> EAST -> LEFT -> NORTH | correct cycle |
| UT-011 | RIGHT wraparound | NORTH -> RIGHT -> EAST -> RIGHT -> SOUTH -> RIGHT -> WEST -> RIGHT -> NORTH | correct cycle |
| UT-012 | Repeated rotation | placed at 2,2,NORTH; LEFT LEFT LEFT LEFT | same orientation as start |
| UT-013 | Valid move north | placed at 0,0,NORTH; MOVE | position becomes 0,1 |
| UT-014 | Move off north edge | placed at 0,4,NORTH; MOVE | state unchanged |
| UT-015 | Move off south edge | placed at 0,0,SOUTH; MOVE | state unchanged |
| UT-016 | Move off east edge | placed at 4,0,EAST; MOVE | state unchanged |
| UT-017 | Move off west edge | placed at 0,0,WEST; MOVE | state unchanged |
| UT-018 | Valid move after blocked move | placed at 0,4,NORTH; MOVE; MOVE; REPORT | first MOVE ignored; second MOVE valid only if still within bounds after move acceptance |
| UT-019 | Repeated PLACE | place 1,1,NORTH; place 2,2,SOUTH | final state = 2,2,SOUTH |
| UT-020 | Move after invalid PLACE | invalid PLACE 5,0,NORTH; MOVE | ignored because robot not placed |

### 6.2 Integration tests: command processing

| Test ID | Scenario | Exact command sequence | Expected output |
| --- | --- | --- | --- |
| IT-001 | Report before placement | REPORT | no valid output |
| IT-002 | Pre-placement move ignored | MOVE | no state change |
| IT-003 | Pre-placement left ignored | LEFT | no state change |
| IT-004 | Pre-placement right ignored | RIGHT | no state change |
| IT-005 | Valid placement and report | PLACE 0,0,NORTH; REPORT | 0,0,NORTH |
| IT-006 | Example 1 | PLACE 0,0,NORTH; MOVE; REPORT | 0,1,NORTH |
| IT-007 | Example 2 | PLACE 0,0,NORTH; LEFT; REPORT | 0,0,WEST |
| IT-008 | Example 3 | PLACE 1,1,EAST; MOVE; MOVE; LEFT; MOVE; REPORT | 3,2,NORTH |
| IT-009 | Invalid placement ignored | PLACE 5,0,NORTH; REPORT | no valid output |
| IT-010 | Boundary guard from north edge | PLACE 0,0,NORTH; MOVE; MOVE; MOVE; MOVE; MOVE; REPORT | 0,4,NORTH |
| IT-011 | Malformed command ignored | JUMP; REPORT | no valid output unless previously placed |
| IT-012 | Empty input | (empty sequence) | no output |
| IT-013 | Partial PLACE syntax | PLACE 0,0; REPORT | no valid output |
| IT-014 | Unknown command after placement | PLACE 0,0,NORTH; JUMP; REPORT | 0,0,NORTH |
| IT-015 | Repeated valid placement | PLACE 0,0,NORTH; PLACE 2,2,EAST; REPORT | 2,2,EAST |
| IT-016 | Repeated rotation | PLACE 1,1,NORTH; LEFT; LEFT; LEFT; LEFT; REPORT | 1,1,NORTH |
| IT-017 | Corner move rejection | PLACE 4,4,NORTH; MOVE; REPORT | 4,4,NORTH |
| IT-018 | Move after blocked move | PLACE 0,4,NORTH; MOVE; MOVE; REPORT | 0,4,NORTH |

## 7. Exact Example Coverage

The supplied assessment examples must be executed exactly and validated by acceptance tests.

### Example A - Basic move
Input:
- PLACE 0,0,NORTH
- MOVE
- REPORT

Expected output:
- 0,1,NORTH

### Example B - Left turn
Input:
- PLACE 0,0,NORTH
- LEFT
- REPORT

Expected output:
- 0,0,WEST

### Example C - Multi-step movement and turn
Input:
- PLACE 1,1,EAST
- MOVE
- MOVE
- LEFT
- MOVE
- REPORT

Expected output:
- 3,2,NORTH

## 8. Proposed Behaviour Tests (Non-Requirement, Review Required)

These tests are not required by the approved assessment but are useful robustness checks if the implementation team chooses to add them. They must be clearly labelled as proposed behaviour and remain secondary to the approved specification.

- lowercase command names: `place 0,0,north`
- lowercase direction tokens: `PLACE 0,0,north`
- command sequence with extra whitespace: `PLACE   0,0,NORTH`
- command sequence with trailing spaces or blank lines
- explicit error reporting for invalid commands (if product requirement later decides to add it)

These proposed tests should not be treated as assessment requirements and should not be used to change the approved contract without review.

## 9. Completion Criteria by Task

Each task is complete when:
- the required component behavior is implemented and verified by tests
- all relevant acceptance criteria for that task are satisfied
- no regression was introduced to earlier tasks
- traceability to requirements remains intact

A task is not complete if any requirement in its scope is untested or contradicted by the current implementation.

## 10. Requirement-to-Task Traceability

| Requirement ID(s) | Tasks | Acceptance Criteria |
| --- | --- | --- |
| FR-001, FR-002, BR-001, BR-002, BR-009 | TASK-001 | AC-001, AC-005, AC-006, AC-013 |
| FR-001, FR-006, BR-001, BR-004, BR-009 | TASK-002 | AC-007, AC-008, AC-013 |
| FR-003, FR-004, FR-005, FR-006, FR-007, FR-008, FR-011, FR-012, BR-003, BR-005, BR-006 | TASK-003 | AC-001, AC-002, AC-003, AC-004, AC-005, AC-006, AC-007, AC-008, AC-011, AC-013 |
| FR-004, FR-005, FR-010, BR-006, BR-007 | TASK-004 | AC-002, AC-003, AC-004, AC-010, AC-012 |
| FR-003, FR-009, FR-010, FR-011, FR-013, BR-003, BR-006, BR-008 | TASK-005 | AC-001, AC-009, AC-010, AC-011, AC-012 |
| FR-009, FR-013, BR-008 | TASK-006 | AC-009, AC-011 |
| All requirements | TASK-007 | All acceptance criteria |
| All requirements | TASK-008 | Final validation and sign-off |

## 11. Test Data and Supporting Files

A supporting test-data file may be created to preserve reusable command sequences and expected results, but no implementation logic should be placed there.

Proposed file:
- `test-data/test-matrix.md`

This file should contain reusable command sequences and expected outcomes for the scenarios listed above.

## 12. Implementation Readiness Gate

Implementation may proceed only if:
- the approved specification is unchanged
- the approved design is accepted
- all tasks above are defined and ordered
- the unit and integration tests above are written or ready to be written
- the requirements traceability matrix passes review

Until then, the project remains in specification/design review and no code should be implemented.
